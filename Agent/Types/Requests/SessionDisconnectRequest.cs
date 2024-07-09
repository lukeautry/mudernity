using Agent.Common;

namespace Agent.Types.Requests
{
    public record SessionDisconnectRequest
    {
        public const string REQUEST_TYPE = "SessionDisconnect";

        public required string Id { get; init; }

        public static SessionDisconnectRequest? Parse(string json)
        {
            return Deserializer.Deserialize<SessionDisconnectRequest>(json);
        }
    }
}