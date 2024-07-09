using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Agent.Socket
{

    public interface IWriter
    {
        Task Write(string data);
    }

    public class Writer(WebSocket webSocket) : IWriter
    {
        private readonly WebSocket webSocket = webSocket;

        public async Task Write(string rawJson)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(rawJson);
                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing to socket: {e.Message}");
            }
        }
    }
}