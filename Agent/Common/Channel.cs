using Agent.Services;

namespace Agent.Common
{
    public interface IChannel<T>
    {
        void Subscribe(Func<T, Task> handler);
        void Unsubscribe(Func<T, Task> handler);
        Task Publish(T message);
    }

    public class Channel<T>(LoggerService logger)
    {
        private readonly List<Func<T, Task>> _handlers = [];
        private readonly LoggerService logger = logger;

        public Func<T, Task> Subscribe(Func<T, Task> handler)
        {
            _handlers.Add(handler);
            return handler;
        }

        public void Unsubscribe(Func<T, Task> handler)
        {
            if (_handlers.Contains(handler))
            {
                try
                {
                    _handlers.Remove(handler);
                }
                catch (Exception e)
                {
                    logger.Error($"Error unsubscribing handler: {e.Message}");
                }
            }
        }

        public void Publish(T message)
        {
            var tasks = _handlers.Select(handler => handler(message));
            Task.WhenAll(tasks);
        }
    }
}