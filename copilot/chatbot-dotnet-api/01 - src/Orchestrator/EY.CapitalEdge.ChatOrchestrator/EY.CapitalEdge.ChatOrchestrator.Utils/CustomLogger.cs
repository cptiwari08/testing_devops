using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    [ExcludeFromCodeCoverage]
    public class CustomLogger : ILogger
    {
        private readonly ILogger _logger;
        private readonly string _prefix;

        public CustomLogger(ILogger logger, string prefix)
        {
            _logger = logger;
            _prefix = prefix;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, (s, e) => $"{_prefix} {formatter(s, e)}");
        }
    }
}
