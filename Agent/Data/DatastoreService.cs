using Agent.Common;
using Agent.Types.Responses;

namespace Agent.Data
{
    public class DatastoreService : IDisposable
    {
        private Datastore _datastore;
        private readonly Channel<SessionList> _sessionChannel;
        private readonly Channel<ProfileList> _profileChannel;
        private readonly WriteManager? _writeManager;

        private const string DbPath = "db.json";
        private const int SaveIntervalMilliseconds = 1000; // Adjust this value to control the interval

        private DatastoreService(Datastore datastore, Channel<Types.Responses.SessionList> sessionChannel, Channel<Types.Responses.ProfileList> profileChannel, bool inMemory)
        {
            _datastore = datastore;
            _sessionChannel = sessionChannel;
            _profileChannel = profileChannel;

            if (!inMemory)
            {
                _writeManager = new WriteManager(SaveIntervalMilliseconds);
            }
        }

        public static async Task<DatastoreService> Initialize(Channel<Types.Responses.SessionList> sessionChannel, Channel<Types.Responses.ProfileList> profileChannel, bool inMemory)
        {
            var datastore = Datastore.GetDefault();
            if (!inMemory)
            {
                Console.WriteLine("Initializing data store...");

                try
                {
                    var db = await File.ReadAllTextAsync(DbPath);

                    if (!string.IsNullOrEmpty(db))
                    {
                        Console.WriteLine($"Reading from {DbPath}...");
                        var result = Deserializer.Deserialize<Datastore>(db);
                        if (result != null)
                        {
                            datastore = result;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading from {DbPath}: {e.Message}");
                }
            }

            return new DatastoreService(datastore, sessionChannel, profileChannel, inMemory);
        }

        /// <summary>
        /// Immutable mutation of the datastore; the original datastore is not modified
        /// </summary>
        public Datastore Mutate(Func<Datastore, Datastore> action)
        {
            var current = _datastore;

            _datastore = action(_datastore);

            if (current.Sessions != _datastore.Sessions)
            {
                _sessionChannel.Publish(new SessionList
                {
                    Sessions = _datastore.Sessions
                });
            }

            if (current.Profiles != _datastore.Profiles)
            {
                _profileChannel.Publish(new ProfileList
                {
                    Profiles = _datastore.Profiles
                });
            }

            _writeManager?.EnqueueWrite(Save);

            return _datastore;
        }

        public void Dispose()
        {
            _writeManager?.Dispose();
        }

        public TResult Query<TResult>(Func<Datastore, TResult> selector)
        {
            return selector(_datastore);
        }

        private async Task Save()
        {
            try
            {
                var json = Serializer.Serialize(_datastore);
                await File.WriteAllTextAsync(DbPath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing to db.json: {e.Message}");
            }
        }
    }
}
