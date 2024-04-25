using NLog;
using Services.Abstraction.ILoggerServices;

namespace Services.Implementation.Extensions
{
    public class LoggerManager() : ILoggerManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message) => logger.Debug(message);
        public void LogError(string message) => logger.Error(message);
        public void LogInfo(string message) => logger.Info(message);
        public void LogWarn(string message) => logger.Warn(message);
    }
}
