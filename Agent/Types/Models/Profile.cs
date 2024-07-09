namespace Agent.Types.Models
{
    public record Profile
    {
        public required string Id { get; init; }

        public required string Name { get; init; }

        public required string Hostname { get; init; }
        public required int Port { get; init; }

        public required int CreatedAt { get; init; }

        public required int UpdatedAt { get; init; }
    }
}