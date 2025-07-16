namespace EY.CE.Copilot.API.Contracts
{
    public interface ISessionClient
    {

        Task<string> SetRedisCache(string key, string value);
        Task<string> GetRedisCache(string key);
        Task<string> DeleteRedisCache(string key);
    }
}
