using EY.CE.Copilot.Data.Contexts;

namespace EY.CE.Copilot.API.Handler
{
    public class FireForgetRepositoryHandler : IFireForgetRepositoryHandler
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public FireForgetRepositoryHandler(IServiceScopeFactory _serviceScopeFactory)
        {
            serviceScopeFactory = _serviceScopeFactory;
        }

        public void Execute(Func<CopilotContext, Task> dbOperation)
        {
            Task.Run(async () =>
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<CopilotContext>();
                    await dbOperation(context);
                }
            });
        }
    }
}
