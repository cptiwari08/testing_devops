using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EY.CapitalEdge.ChatOrchestrator.Tests.Fakes
{
    internal class FakeOrchestrationMetadataAsyncPageable : AsyncPageable<OrchestrationMetadata>
    {
        public override IAsyncEnumerable<Page<OrchestrationMetadata>> AsPages(string continuationToken = null, int? pageSizeHint = null)
        {
            return AsyncEnumerable.Empty<Page<OrchestrationMetadata>>();
        }
    }
}
