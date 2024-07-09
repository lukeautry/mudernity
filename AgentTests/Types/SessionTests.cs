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

            var expectedData = "{\"type\":\"SessionList\",\"sessions\":[{\"id\":\"test\",\"hostname\":\"test\",\"state\":\"active\",\"port\":1234}]}";

            Assert.Equal(expectedData, json);
        }
    }
}