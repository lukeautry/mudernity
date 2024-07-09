using Agent.Common;

namespace Agent.Types.Requests
{
    public record SessionCommandRequest
    {
        public const string REQUEST_TYPE = "SessionCommand";

        public required string Id { get; init; }

        public required string Command { get; init; }

        public static SessionCommandRequest? Parse(string json)
        {
            return Deserializer.Deserialize<SessionCommandRequest>(json);
        }
    }
}