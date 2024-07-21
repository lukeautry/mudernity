using Agent.Common;
using Agent.Types.Models;
using Agent.Types.Responses;

namespace AgentTests.Types
{
    public class SessionTests
    {
        [Fact]
        public void Test_CanConvert()
        {
            var result = new SessionList
            {
                Sessions =
                [
                    new Session
                    {
                        Id = "test",
                        Hostname = "test",
                        State = SessionState.Active,
                        Port = 1234,
                    }
                ]
            };

            var json = result.ToJson();

            var deserialized = Deserializer.Deserialize<SessionList>(json);

            Assert.NotNull(deserialized);
            Assert.Equal(result.Sessions.Count, deserialized.Sessions.Count);
            Assert.Equal(result.Sessions[0].Id, deserialized.Sessions[0].Id);
            Assert.Equal(result.Sessions[0].Hostname, deserialized.Sessions[0].Hostname);
            Assert.Equal(result.Sessions[0].State, deserialized.Sessions[0].State);
            Assert.Equal(result.Sessions[0].Port, deserialized.Sessions[0].Port);
        }
    }
}