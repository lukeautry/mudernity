using Agent.Types.Requests;

namespace AgentTests.Types
{
    public class ProfileCreationRequestTests
    {
        [Fact]
        public void Test_CanParse()
        {
            string json = @"{
                ""type"":""ProfileCreation"",
                ""hostname"":""localhost"",
                ""port"":8080,
                ""name"":""test""
            }";

            var result = ProfileCreationRequest.Parse(json);
            if (result == null)
            {
                throw new Exception("Result is null");
            }
            else
            {
                Assert.Equal("ProfileCreation", result.Type);
                Assert.Equal("localhost", result.Hostname);
                Assert.Equal(8080, result.Port);
                Assert.Equal("test", result.Name);
            }

        }
    }
}