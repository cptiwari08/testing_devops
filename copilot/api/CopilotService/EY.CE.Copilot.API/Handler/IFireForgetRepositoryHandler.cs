using EY.CE.Copilot.Data.Contexts;

namespace EY.CE.Copilot.API.Handler
{
    public interface IFireForgetRepositoryHandler
    {
        void Execute(Func<CopilotContext, Task> dbOperation);
    }
}
