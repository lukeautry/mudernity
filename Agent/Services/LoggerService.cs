namespace Agent.Services
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }

    public class LoggerService(LogLevel minLogLevel = LogLevel.Information)
    {
        private readonly LogLevel _minLogLevel = minLogLevel;

        private void Log(LogLevel level, string message)
        {
            if (level < _minLogLevel)
                return;

            var logLevelString = level.ToString().ToUpper();
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"[{timestamp}] [{logLevelString}] {message}");
        }

        public void Trace(string message) => Log(LogLevel.Trace, message);
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Information(string message) => Log(LogLevel.Information, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message) => Log(LogLevel.Error, message);
        public void Critical(string message) => Log(LogLevel.Critical, message);
    }
}