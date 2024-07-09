using Agent.Common;

namespace Agent.Types.Requests
{
    public record ProfileDeletionRequest
    {
        public const string REQUEST_TYPE = "ProfileDeletion";

        public required string Id { get; init; }

        public static ProfileDeletionRequest? Parse(string json)
        {
            return Deserializer.Deserialize<ProfileDeletionRequest>(json);
        }
    }
}