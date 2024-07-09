using System.Collections.Concurrent;

namespace Agent.Data
{
    public class WriteManager : IDisposable
    {
        private readonly ConcurrentQueue<Func<Task>> _writeQueue;
        private readonly System.Timers.Timer _timer;
        private readonly int _intervalMilliseconds;

        public WriteManager(int intervalMilliseconds)
        {
            _writeQueue = new ConcurrentQueue<Func<Task>>();
            _intervalMilliseconds = intervalMilliseconds;
            _timer = new System.Timers.Timer(_intervalMilliseconds);
            _timer.Elapsed += async (sender, e) => await ProcessQueue();
            _timer.Start();
        }

        public void EnqueueWrite(Func<Task> writeAction)
        {
            _writeQueue.Enqueue(writeAction);
        }

        private async Task ProcessQueue()
        {
            while (_writeQueue.TryDequeue(out var writeAction))
            {
                await writeAction();
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
