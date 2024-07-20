using Agent.Types.Requests;

namespace Agent.Services
{
    public class SessionService(ProgramContext context)
    {
        internal async Task HandleCloseSession(string message)
        {
            var request = SessionCloseRequest.Parse(message);

            if (request != null)
            {
                context.SessionRepository.DeleteSession(request.Id);
                await context.ConnectionPool.CloseSessionConnection(request.Id);
            }
        }

        internal async Task HandleConnectSession(string message)
        {
            var request = SessionConnectRequest.Parse(message);
            if (request != null)
            {
                await ConnectSession(request.Id);
            }
        }

        internal void ResumeSessions()
        {
            var sessions = context.SessionRepository.GetSessions();

            foreach (var session in sessions)
            {
                context.ConnectionPool.Resume(session.Id);
            }
        }

        internal async Task HandleDisconnectSession(string message)
        {
            var request = SessionDisconnectRequest.Parse(message);
            if (request != null)
            {
                context.ConnectionPool.Disconnect(request.Id);
            }
        }

        private async Task ConnectSession(string sessionId)
        {
            var session = context.SessionRepository.GetSession(sessionId);
            if (session != null)
            {
                // purposely not awaiting this task
                context.ConnectionPool.Connect(session);
            }
        }

        internal async Task HandleCommand(string message)
        {
            var request = SessionCommandRequest.Parse(message);
            if (request == null)
            {
                return;
            }

            var session = context.SessionRepository.GetSession(request.Id);
            if (session == null)
            {
                return;
            }

            var delimiter = ";";

            var commands = request.Command.Split(delimiter);

            foreach (var command in commands)
            {
                await context.ConnectionPool.SendCommand(session.Id, command);
            }
        }
    }
}