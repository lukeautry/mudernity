using Agent.Common;

namespace Agent.Types.Requests
{
    public record ProfileConnectionRequest
    {
        public const string REQUEST_TYPE = "ProfileConnection";

        public required string Id { get; init; }

        public static ProfileConnectionRequest? Parse(string json)
        {
            return Deserializer.Deserialize<ProfileConnectionRequest>(json);
        }
    }
}