using System.Net;
using System.Net.WebSockets;
using System.Text;
using Agent;
using Agent.Common;
using Agent.Data;
using Agent.Services;
using Agent.Socket;
using Agent.Types.Responses;

Console.WriteLine("Agent starting...");

var sessionChannel = new Channel<SessionList>();
var profileChannel = new Channel<ProfileList>();
var sessionDataChannel = new Channel<SessionData>();
var store = await DatastoreService.Initialize(sessionChannel, profileChannel, false);
var sessionRepository = new SessionRepository(store);
var connectionPool = new ConnectionPoolService(sessionRepository, sessionDataChannel);
var profileRepository = new ProfileRepository(store);

var httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:5000/");
httpListener.Start();

Console.WriteLine("Agent serving at at ws://localhost:5000");

async Task AcceptWebSocketClients(HttpListener listener)
{
    while (true)
    {
        var httpContext = await listener.GetContextAsync();

        if (httpContext.Request.IsWebSocketRequest)
        {
            var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
            var webSocket = webSocketContext.WebSocket;

            Console.WriteLine("Agent connected to a client");

            await HandleWebSocketConnection(webSocket);
        }
        else
        {
            httpContext.Response.StatusCode = 400;
            httpContext.Response.Close();
        }
    }
}

async Task HandleWebSocketConnection(WebSocket webSocket)
{
    var writer = new Writer(webSocket);
    var reader = new Reader(webSocket);

    var context = new ProgramContext
    {
        Writer = writer,
        Reader = reader,
        Store = store,
        ConnectionPool = connectionPool,
        SessionRepository = sessionRepository,
        ProfileRepository = profileRepository
    };

    var sessionHandler = sessionChannel.Subscribe(async sessionList =>
    {
        await writer.Write(sessionList.ToJson());
    });

    await sessionHandler(context.SessionRepository.GetSessionList());

    var profileHandler = profileChannel.Subscribe(async profileList =>
    {
        await writer.Write(profileList.ToJson());
    });

    await profileHandler(context.ProfileRepository.GetProfileList());

    var sessionDataHandler = sessionDataChannel.Subscribe(async sessionData =>
    {
        await writer.Write(sessionData.ToJson());
    });

    var processor = new Processor(context);

    await processor.Start();

    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

    sessionChannel.Unsubscribe(sessionHandler);
    profileChannel.Unsubscribe(profileHandler);
    sessionDataChannel.Unsubscribe(sessionDataHandler);
}

await AcceptWebSocketClients(httpListener);
