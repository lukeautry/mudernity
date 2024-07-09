namespace Agent.Types.Models
{
    public record Session
    {
        public required string Id { get; init; }
        public required string Hostname { get; init; }
        public required SessionState State { get; init; }
        public required int Port { get; init; }
    }

    public enum SessionState
    {
        Active,
        Inactive
    }
}