using System.Net;
using System.Net.WebSockets;
using Agent;
using Agent.Common;
using Agent.Data;
using Agent.Services;
using Agent.Socket;
using Agent.Types.Responses;

var logger = new LoggerService(LogLevel.Debug);
var sessionChannel = new Channel<SessionList>(logger);
var profileChannel = new Channel<ProfileList>(logger);
var sessionDataChannel = new Channel<SessionData>(logger);
var store = await DatastoreService.Initialize(sessionChannel, profileChannel, logger, false);
var sessionRepository = new SessionRepository(store);
var connectionPool = new ConnectionPoolService(sessionRepository, sessionDataChannel, logger);
var profileRepository = new ProfileRepository(store);

var httpListener = new HttpListener();
httpListener.Prefixes.Add("http://localhost:5000/");
httpListener.Start();

logger.Information($"Agent serving at http://localhost:5000");

var staticFileServer = new StaticFileServer();

async Task ProcessRequests(HttpListener listener)
{
    while (true)
    {
        var httpContext = await listener.GetContextAsync();

        if (httpContext.Request.IsWebSocketRequest)
        {
            var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
            var webSocket = webSocketContext.WebSocket;

            _ = HandleWebSocketConnection(webSocket); // Start WebSocket handling
        }
        else
        {
            _ = staticFileServer.ServeStaticFileAsync(httpContext); // Serve static files
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

await ProcessRequests(httpListener);
