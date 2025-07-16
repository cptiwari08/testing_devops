using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator.Tests.Fakes
{
    public abstract class FakeTaskOrchestrationContext : TaskOrchestrationContext
    {

        public override TaskName Name => throw new NotImplementedException();

        public override string InstanceId => Guid.NewGuid().ToString();

        public override ParentOrchestrationInstance? Parent => throw new NotImplementedException();

        public override DateTime CurrentUtcDateTime => throw new NotImplementedException();

        public override bool IsReplaying => throw new NotImplementedException();

        protected override ILoggerFactory LoggerFactory => throw new NotImplementedException();

        public override Task<TResult> CallActivityAsync<TResult>(TaskName name, object? input = null, TaskOptions? options = null)
        {
            return Task.FromResult(default(TResult));
        }

        public override Task<TResult> CallSubOrchestratorAsync<TResult>(TaskName orchestratorName, object? input = null, TaskOptions? options = null)
        {
            throw new NotImplementedException();
        }

        public override void ContinueAsNew(object? newInput = null, bool preserveUnprocessedEvents = true)
        {
            throw new NotImplementedException();
        }

        public override Task CreateTimer(DateTime fireAt, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override T? GetInput<T>() where T : default
        {
            return default;
        }

        public override Guid NewGuid()
        {
            throw new NotImplementedException();
        }

        public override void SendEvent(string instanceId, string eventName, object payload)
        {
            throw new NotImplementedException();
        }

        public override void SetCustomStatus(object? customStatus)
        {
            throw new NotImplementedException();
        }

        public override Task<T> WaitForExternalEvent<T>(string eventName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override ILogger CreateReplaySafeLogger<T>()
        {
            return new FakeLogger();
        }
    }
}
