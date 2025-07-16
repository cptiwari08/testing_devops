using EY.CE.Copilot.API.Handler;
using EY.CE.Copilot.Data.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantAPITest.Dependency
{
    public class FireForgetRepositoryHandler : IFireForgetRepositoryHandler
    {
        public FireForgetRepositoryHandler()
        {
        }

        public void Execute(Func<CopilotContext, Task> dbOperation)
        {
            Task.Run(async () =>
            {
                var context = new CopilotContextDependency().context;
                await dbOperation(context);
            });
        }
    }
}
