using Agent.Common;
using Agent.Types.Models;
using Agent.Types.Requests;

namespace Agent.Services
{
    public class ProfileService(ProgramContext context)
    {
        internal async Task HandleCreateProfile(string message)
        {
            var request = Deserializer.Deserialize<ProfileCreationRequest>(message);
            if (request != null)
            {
                var currentDate = DateUtilities.UtcTimestamp();

                context.ProfileRepository.AddProfile(new Profile
                {
                    Id = IdGenerator.Generate(),
                    Port = request.Port,
                    Hostname = request.Hostname,
                    Name = request.Name,
                    CreatedAt = currentDate,
                    UpdatedAt = currentDate
                });
            }
        }

        internal async Task HandleDeleteProfile(string message)
        {
            var request = ProfileDeletionRequest.Parse(message);
            if (request != null)
            {
                context.ProfileRepository.DeleteProfile(request.Id);
            }
        }

        internal async Task HandleUpdateProfile(string message)
        {
            var request = ProfileUpdateRequest.Parse(message);

            if (request != null)
            {
                context.ProfileRepository.UpdateProfile(request.Profile);
            }
        }

        internal async Task HandleConnectProfile(string message)
        {
            var request = ProfileConnectionRequest.Parse(message);

            if (request != null)
            {
                var profile = context.ProfileRepository.GetProfile(request.Id);
                if (profile != null)
                {
                    var session = new Session
                    {
                        Id = IdGenerator.Generate(),
                        Hostname = profile.Hostname,
                        Port = profile.Port,
                        State = SessionState.Inactive,
                    };

                    context.SessionRepository.AddSession(session);

                    context.ConnectionPool.Connect(session);
                }
            }
        }
    }
}