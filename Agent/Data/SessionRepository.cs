using Agent.Types.Models;
using Agent.Types.Responses;

namespace Agent.Data
{
    public class SessionRepository(DatastoreService store)
    {
        public SessionList GetSessionList()
        {
            return new SessionList
            {
                Sessions = GetSessions()
            };
        }

        public List<Session> GetSessions()
        {
            return store.Query(store => store.Sessions);
        }

        public Session? GetSession(string id)
        {
            return GetSessions().FirstOrDefault(session => session.Id == id);
        }

        public void AddSession(Session session)
        {
            store.Mutate(store =>
            {
                return store with
                {
                    Sessions = [.. store.Sessions, session]
                };
            });
        }

        public void DeleteSession(string sessionId)
        {
            store.Mutate(store =>
            {
                return store with
                {
                    Sessions = store.Sessions.Where(session => session.Id != sessionId).ToList()
                };
            });
        }

        public void UpdateState(string sessionId, SessionState state)
        {
            store.Mutate(store =>
            {
                var sessions = store.Sessions.Select(session =>
                {
                    if (session.Id == sessionId)
                    {
                        return session with
                        {
                            State = state
                        };
                    }

                    return session;
                }).ToList();

                return store with
                {
                    Sessions = sessions
                };
            });
        }

        public List<Session> GetActiveSessions()
        {
            return GetSessions().Where(session => session.State == SessionState.Active).ToList();
        }
    }
}