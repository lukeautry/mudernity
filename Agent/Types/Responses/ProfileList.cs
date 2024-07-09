using Agent.Common;
using Agent.Types.Models;

namespace Agent.Types.Responses
{
    public record ProfileList
    {
        public const string RESPONSE_TYPE = "ProfileList";

        public string Type { get; init; } = RESPONSE_TYPE;
        public required List<Profile> Profiles { get; set; }

        public string ToJson()
        {
            return Serializer.Serialize(this);
        }
    }
}