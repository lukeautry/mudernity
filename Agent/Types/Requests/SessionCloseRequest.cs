using Agent.Common;

namespace Agent.Types.Requests
{
    public record SessionCloseRequest
    {
        public const string REQUEST_TYPE = "SessionClose";

        public required string Id { get; init; }

        public static SessionCloseRequest? Parse(string json)
        {
            return Deserializer.Deserialize<SessionCloseRequest>(json);
        }
    }
}