using Agent.Common;
using Agent.Types.Models;

namespace Agent.Types.Responses
{
    public record SessionList
    {
        public const string RESPONSE_TYPE = "SessionList";

        public string Type { get; init; } = RESPONSE_TYPE;
        public required List<Session> Sessions { get; set; }

        public string ToJson()
        {
            return Serializer.Serialize(this);
        }
    }
}