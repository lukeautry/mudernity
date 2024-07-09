using Agent.Common;

namespace Agent.Types
{
    public record IncomingMessage
    {
        public required string Type { get; init; }

        public static IncomingMessage? Parse(string json)
        {
            return Deserializer.Deserialize<IncomingMessage>(json);
        }
    }
}