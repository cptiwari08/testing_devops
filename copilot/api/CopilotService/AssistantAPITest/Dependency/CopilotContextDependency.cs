using EY.CE.Copilot.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AssistantAPITest.Dependency
{
    internal class CopilotContextDependency
    {
        DbContextOptionsBuilder<CopilotContext> builder = new DbContextOptionsBuilder<CopilotContext>();
        internal CopilotContext context;
        string localDbconnectionString = $@"Server=(LocalDB)\MSSQLLocalDB; Database=ce5-local-copilot; Integrated Security=True";
        public CopilotContextDependency()
        {
            builder.UseSqlServer(localDbconnectionString);
            context = new CopilotContext(builder.Options);
        }
    }
}
