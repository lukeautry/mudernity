using System.Net.Sockets;
using System.Text;
using Agent.Common;
using Agent.Data;
using Agent.Types.Models;
using Agent.Types.Responses;
using Agent.Utilities;

namespace Agent.Services
{
    /// <summary>
    /// Service for managing connection pools to MUD servers. Connections are pooled by session ID.
    /// Since sessions can go away and come back, we need to be able to re-use connections.
    /// </summary>
    public class ConnectionPoolService(SessionRepository sessionRepository, Channel<SessionData> sessionDataChannel)
    {
        private readonly Dictionary<string, (TcpClient client, NetworkStream stream)> _connections = [];

        private readonly Dictionary<string, byte[]> _buffers = [];

        public async void Connect(Session session)
        {
            if (_connections.ContainsKey(session.Id))
            {
                Console.WriteLine($"Already connected to the server for session {session.Id}.");

                // output existing buffer if it exists
                Console.WriteLine($"Outputting existing buffer for session {session.Id}.");

                var buffer = _buffers.GetValueOrDefault(session.Id, []);
                if (buffer.Length > 0)
                {
                    PublishSessionData(session.Id, buffer);
                }

                return;
            }

            try
            {
                var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(session.Hostname, session.Port);

                sessionRepository.UpdateState(session.Id, SessionState.Active);

                var networkStream = tcpClient.GetStream();

                _connections[session.Id] = (tcpClient, networkStream);

                Console.WriteLine($"Connected to the MUD server for session {session.Id}.");

                // print out a green message that says something like # CONNECTED TO HOSTNAME:PORT
                PublishSessionData(session.Id, new AnsiMessageBuilder()
                    .AddText($"# CONNECTED TO {session.Hostname}:{session.Port}", AnsiMessageBuilder.Color.BrightGreen)
                    .AddNewLine()
                    .Build());

                // Start reading from the server
                await ReadFromServer(session.Id, networkStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the server for session {session.Id}: {ex.Message}");
                sessionRepository.UpdateState(session.Id, SessionState.Inactive);

                CleanupSession(session.Id);
            }
        }

        private void PublishSessionData(string sessionId, byte[] data)
        {
            var sessionData = new SessionData
            {
                SessionId = sessionId,
                Data = data
            };

            sessionDataChannel.Publish(sessionData);
        }

        private async Task ReadFromServer(string sessionId, NetworkStream networkStream)
        {
            var buffer = new byte[4096];
            int bytesRead;

            try
            {
                while ((bytesRead = await networkStream.ReadAsync(buffer)) != 0)
                {
                    var byteArr = new byte[bytesRead];

                    Array.Copy(buffer, byteArr, bytesRead);

                    var existingBuffer = _buffers.GetValueOrDefault(sessionId, []);
                    var newBuffer = existingBuffer.Concat(byteArr).ToArray();

                    _buffers[sessionId] = newBuffer;

                    PublishSessionData(sessionId, byteArr);
                }

                Console.WriteLine($"MUD server disconnected for session {sessionId}.");
                Disconnect(sessionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from the server for session {sessionId}: {ex.Message}");
            }
        }

        public void Disconnect(string sessionId)
        {
            if (_connections.TryGetValue(sessionId, out var connection))
            {
                connection.stream?.Close();
                sessionRepository.UpdateState(sessionId, SessionState.Inactive);

                _connections.Remove(sessionId);

                PublishSessionData(sessionId, new AnsiMessageBuilder()
                    .AddText("# DISCONNECTED FROM SERVER", AnsiMessageBuilder.Color.BrightRed)
                    .AddNewLine()
                    .Build());

                Console.WriteLine($"Disconnected from the MUD server for session {sessionId}.");
            }
            else
            {
                Console.WriteLine($"No active connection found for session {sessionId}.");
            }
        }

        internal async Task CloseSessionConnection(string id)
        {
            if (_connections.TryGetValue(id, out var connection))
            {
                connection.stream?.Close();
                connection.client?.Close();
                _connections.Remove(id);
                _buffers.Remove(id);

                Console.WriteLine($"Disconnected from the MUD server for session {id}.");
            }
            else
            {
                Console.WriteLine($"No active connection found for session {id}.");
            }
        }

        private void CleanupSession(string sessionId)
        {
            if (_connections.TryGetValue(sessionId, out var connection))
            {
                connection.stream?.Close();
                connection.client?.Close();
                _connections.Remove(sessionId);
            }
        }

        /// <summary>
        /// Resume a session connection
        /// Checks to see if there is an existing connection for the session ID
        /// If there is, pull from the buffer and send to the session data channel
        /// </summary>
        internal void Resume(string sessionId)
        {
            var buffer = _buffers.GetValueOrDefault(sessionId, []);
            if (buffer.Length > 0)
            {
                PublishSessionData(sessionId, buffer);
            }

            if (!_connections.ContainsKey(sessionId))
            {
                Console.WriteLine($"No active connection found for session {sessionId}.");
                sessionRepository.UpdateState(sessionId, SessionState.Inactive);
                return;
            }

            var (client, _) = _connections[sessionId];
            // check if this connection is still active
            if (!client.Connected)
            {
                Console.WriteLine($"Connection for session {sessionId} is no longer active.");
                sessionRepository.UpdateState(sessionId, SessionState.Inactive);
                return;
            }

            Console.WriteLine($"Resuming connection for session {sessionId}.");
        }

        internal async Task SendCommand(string id, string command)
        {
            if (_connections.TryGetValue(id, out var connection))
            {
                var buffer = Encoding.ASCII.GetBytes(command + "\n");

                try
                {
                    await connection.stream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending command to the server for session {id}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"No active connection found for session {id}.");
            }
        }
    }
}