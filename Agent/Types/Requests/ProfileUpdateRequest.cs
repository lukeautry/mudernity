using Agent.Common;
using Agent.Types.Models;

namespace Agent.Types.Requests
{
    public record ProfileUpdateRequest
    {
        public const string REQUEST_TYPE = "ProfileUpdate";

        public required Profile Profile { get; init; }

        public static ProfileUpdateRequest? Parse(string json)
        {
            return Deserializer.Deserialize<ProfileUpdateRequest>(json);
        }
    }
}