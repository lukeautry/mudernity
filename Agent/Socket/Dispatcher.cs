using Agent.Services;
using Agent.Types;
using Agent.Types.Requests;

namespace Agent.Socket
{
    public interface IDispatcher
    {
        Task Dispatch(string message);
    }

    public class Dispatcher
    {
        private readonly Dictionary<string, Func<string, Task>> handlers;

        public Dispatcher(ProgramContext context)
        {
            var sessionService = new SessionService(context);
            var profileService = new ProfileService(context);

            handlers = new Dictionary<string, Func<string, Task>>
            {
                { ProfileCreationRequest.REQUEST_TYPE, profileService.HandleCreateProfile },
                { ProfileDeletionRequest.REQUEST_TYPE,  profileService.HandleDeleteProfile },
                { ProfileUpdateRequest.REQUEST_TYPE, profileService.HandleUpdateProfile },
                { ProfileConnectionRequest.REQUEST_TYPE, profileService.HandleConnectProfile },
                { SessionConnectRequest.REQUEST_TYPE, sessionService.HandleConnectSession },
                { SessionDisconnectRequest.REQUEST_TYPE, sessionService.HandleDisconnectSession },
                { SessionCloseRequest.REQUEST_TYPE, sessionService.HandleCloseSession },
                { SessionCommandRequest.REQUEST_TYPE, sessionService.HandleCommand }
            };

            sessionService.ResumeSessions();
        }

        public async Task Dispatch(string message)
        {
            var incomingMessage = IncomingMessage.Parse(message);
            if (incomingMessage == null)
            {
                return;
            }

            var handler = handlers.GetValueOrDefault(incomingMessage.Type);
            if (handler != null)
            {
                await handler(message);
            }
            else
            {
                Console.WriteLine($"No handler found for message type: {incomingMessage.Type}");
            }
        }
    }
}