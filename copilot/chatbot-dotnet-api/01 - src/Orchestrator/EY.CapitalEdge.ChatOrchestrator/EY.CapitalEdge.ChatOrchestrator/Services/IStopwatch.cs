namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface IStopwatch
    {
        void Start();
        void Stop();
        TimeSpan Elapsed { get; }
    }
}
