using System.Text.Json.Serialization;
using Agent.Data;
using Agent.Types;
using Agent.Types.Models;
using Agent.Types.Requests;
using Agent.Types.Responses;

// PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//             Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
//             WriteIndented = true

namespace Agent.Common
{
    [JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(IncomingMessage))]
    [JsonSerializable(typeof(ProfileList))]
    [JsonSerializable(typeof(SessionList))]
    [JsonSerializable(typeof(SessionData))]
    [JsonSerializable(typeof(ProfileConnectionRequest))]
    [JsonSerializable(typeof(ProfileCreationRequest))]
    [JsonSerializable(typeof(ProfileDeletionRequest))]
    [JsonSerializable(typeof(ProfileUpdateRequest))]
    [JsonSerializable(typeof(SessionCloseRequest))]
    [JsonSerializable(typeof(SessionCommandRequest))]
    [JsonSerializable(typeof(SessionConnectRequest))]
    [JsonSerializable(typeof(SessionDisconnectRequest))]
    [JsonSerializable(typeof(Profile))]
    [JsonSerializable(typeof(Session))]
    [JsonSerializable(typeof(SessionState))]
    [JsonSerializable(typeof(Datastore))]
    public partial class SerializerContext : JsonSerializerContext
    {
    }
}
