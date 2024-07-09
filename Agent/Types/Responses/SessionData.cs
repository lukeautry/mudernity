using Agent.Common;

namespace Agent.Types.Responses
{
    public class SessionData
    {
        public const string RESPONSE_TYPE = "SessionData";

        public string Type { get; init; } = RESPONSE_TYPE;

        public required string SessionId { get; set; }

        public required byte[] Data { get; set; }

        public string ToJson()
        {
            return Serializer.Serialize(this);
        }
    }
}