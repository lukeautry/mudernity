using Agent.Types;

namespace AgentTests.Types;

public class IncomingMessageTests
{
    [Fact]
    public void Test_CanParse()
    {
        var result = IncomingMessage.Parse("{\"type\":\"test\"}");

        Assert.NotNull(result);

        Assert.Equal("test", result.Type);
    }

    [Fact]
    public void Test_FailureReturnsNull()
    {
        var result = IncomingMessage.Parse("{\"someother\":\"test\"");

        Assert.Null(result);
    }
}