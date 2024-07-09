using Agent.Types.Models;

namespace Agent.Data
{

    public record Datastore
    {
        public required List<Session> Sessions { get; set; }

        public required List<Profile> Profiles { get; set; }

        public static Datastore GetDefault()
        {
            return new Datastore
            {
                Sessions = [],
                Profiles = []
            };
        }
    }
}