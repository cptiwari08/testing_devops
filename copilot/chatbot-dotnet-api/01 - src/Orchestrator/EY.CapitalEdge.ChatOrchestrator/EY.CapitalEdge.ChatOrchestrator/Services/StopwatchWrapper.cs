using System.Diagnostics;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public class StopwatchWrapper : IStopwatch
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public void Start() => _stopwatch.Start();
        public void Stop() => _stopwatch.Stop();
        public TimeSpan Elapsed => _stopwatch.Elapsed;
    }
}
