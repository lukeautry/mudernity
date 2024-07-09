using Agent.Common;

namespace Agent.Types.Requests
{
    public record ProfileCreationRequest
    {
        public const string REQUEST_TYPE = "ProfileCreation";

        public string Type { get; init; } = REQUEST_TYPE;

        public required string Hostname { get; init; }
        public required int Port { get; init; }

        public required string Name { get; init; }

        public static ProfileCreationRequest? Parse(string json)
        {
            return Deserializer.Deserialize<ProfileCreationRequest>(json);
        }
    }
}