using System.Net.WebSockets;

namespace Agent.Socket
{
    public interface IReader
    {
        Task<string?> Read();
    }

    public class Reader(WebSocket webSocket) : IReader
    {
        private readonly WebSocket webSocket = webSocket;

        public async Task<string?> Read()
        {
            var buffer = new byte[1024 * 4];

            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    return System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading from WebSocket: {e.Message}");
            }

            return null;
        }
    }
}