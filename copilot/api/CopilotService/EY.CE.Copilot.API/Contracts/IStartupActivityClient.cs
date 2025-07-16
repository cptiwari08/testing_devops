namespace EY.CE.Copilot.API.Contracts
{
    public interface IStartupActivityClient
    {
        Task<bool> Execute();
    }
}
