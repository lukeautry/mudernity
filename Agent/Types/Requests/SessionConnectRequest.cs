using Agent.Common;

namespace Agent.Types.Requests
{
    public record SessionConnectRequest
    {
        public const string REQUEST_TYPE = "SessionConnect";

        public required string Id { get; init; }

        public static SessionConnectRequest? Parse(string json)
        {
            return Deserializer.Deserialize<SessionConnectRequest>(json);
        }
    }
}