using Agent.Common;
using Agent.Data;
using Agent.Services;
using Agent.Socket;
using Agent.Types.Responses;

namespace Agent
{
    public class ProgramContext
    {
        public required IWriter Writer { get; init; }

        public required IReader Reader { get; init; }

        public required DatastoreService Store { get; init; }

        public required ConnectionPoolService ConnectionPool { get; init; }

        public required SessionRepository SessionRepository { get; init; }

        public required ProfileRepository ProfileRepository { get; init; }
    }
}