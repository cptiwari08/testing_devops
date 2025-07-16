using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;

namespace EY.CapitalEdge.ChatOrchestrator.Tests.Fakes
{
    public class FakeDurableTaskClient : DurableTaskClient
    {
        public FakeDurableTaskClient() : base("fake")
        {
        }

        public override ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public override AsyncPageable<OrchestrationMetadata> GetAllInstancesAsync(OrchestrationQuery? filter = null)
        {
            return new FakeOrchestrationMetadataAsyncPageable();
        }

        public override Task<OrchestrationMetadata?> GetInstancesAsync(string instanceId, bool getInputsAndOutputs = false, CancellationToken cancellation = default)
        {
            return Task.FromResult(new OrchestrationMetadata(Guid.NewGuid().ToString(), instanceId));
        }

        public override Task RaiseEventAsync(string instanceId, string eventName, object? eventPayload = null, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }

        public override Task ResumeInstanceAsync(string instanceId, string? reason = null, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }

        public override Task<string> ScheduleNewOrchestrationInstanceAsync(TaskName orchestratorName, CancellationToken cancellation)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public override Task<string> ScheduleNewOrchestrationInstanceAsync(TaskName orchestratorName, object? input, CancellationToken cancellation)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public override Task<string> ScheduleNewOrchestrationInstanceAsync(TaskName orchestratorName, object? input = null, StartOrchestrationOptions? options = null, CancellationToken cancellation = default)
        {
            return Task.FromResult(options?.InstanceId ?? Guid.NewGuid().ToString());
        }

        public override Task SuspendInstanceAsync(string instanceId, string? reason = null, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }

        public override Task<OrchestrationMetadata> WaitForInstanceCompletionAsync(string instanceId, bool getInputsAndOutputs = false, CancellationToken cancellation = default)
        {
            return Task.FromResult(new OrchestrationMetadata(Guid.NewGuid().ToString(), instanceId));
        }

        public override Task<OrchestrationMetadata> WaitForInstanceStartAsync(string instanceId, bool getInputsAndOutputs = false, CancellationToken cancellation = default)
        {
            return Task.FromResult(new OrchestrationMetadata(Guid.NewGuid().ToString(), instanceId));
        }
    }
}
