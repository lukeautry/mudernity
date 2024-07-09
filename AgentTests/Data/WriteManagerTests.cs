using Agent.Data;

namespace AgentTests.Data
{
    public class WriteManagerTests : IDisposable
    {
        private readonly WriteManager _writeManager;
        private readonly List<int> _writeResults;
        private readonly int _intervalMilliseconds = 500; // Adjust this value as needed for the test

        public WriteManagerTests()
        {
            _writeManager = new WriteManager(_intervalMilliseconds);
            _writeResults = [];
        }

        [Fact]
        public async Task EnqueueWrite_WritesAreProcessed()
        {
            // Arrange
            var expectedWrites = new List<int> { 1, 2, 3 };

            foreach (var write in expectedWrites)
            {
                _writeManager.EnqueueWrite(() => WriteAction(write));
            }

            // Act
            // Wait for the interval to ensure the writes are processed
            await Task.Delay(_intervalMilliseconds * 2);

            // Assert
            Assert.Equal(expectedWrites, _writeResults);
        }

        [Fact]
        public async Task EnqueueWrite_WritesAreProcessedInOrder()
        {
            // Arrange
            var expectedWrites = new List<int> { 1, 2, 3 };

            foreach (var write in expectedWrites)
            {
                _writeManager.EnqueueWrite(() => WriteAction(write));
            }

            // Act
            // Wait for the interval to ensure the writes are processed
            await Task.Delay(_intervalMilliseconds * 2);

            // Assert
            Assert.Equal(expectedWrites, _writeResults);
        }

        [Fact]
        public async Task EnqueueWrite_WritesNotProcessedPriorToInterval()
        {
            // Arrange
            var expectedWrites = new List<int> { 1, 2, 3 };

            foreach (var write in expectedWrites)
            {
                _writeManager.EnqueueWrite(() => WriteAction(write));
            }

            // Act
            // Wait for less than the interval to ensure the writes are not processed
            await Task.Delay(_intervalMilliseconds / 2);

            // Assert
            Assert.Empty(_writeResults);
        }

        private async Task WriteAction(int value)
        {
            // Simulate an asynchronous write operation
            await Task.Delay(50);
            _writeResults.Add(value);
        }

        public void Dispose()
        {
            _writeManager.Dispose();
        }
    }
}
