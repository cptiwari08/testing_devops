using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using static EY.CE.Copilot.API.Static.Constants;

namespace EY.CE.Copilot.API.Clients
{
    public class SessionClient(IAppLoggerService logger, IRedisClient redisClient) : BaseClass(logger, nameof(SessionClient)), ISessionClient
    {
        public async Task<string> SetRedisCache(string key, string value)
        {
            try
            {
                redisClient.ConnectRedisCache();
                await RedisClient.cache.StringSetAsync(key, value,TimeSpan.FromMinutes(Redis.RedisTimeout));
                return Constants.Redis.OperationStatus.Success;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method SetRedisCache - {e.Message}", nameof(SetRedisCache), exception: e);
                return Constants.Redis.OperationStatus.Failure;
            }
        }

        public async Task<string> GetRedisCache(string key)
        {
            try
            {
                redisClient.ConnectRedisCache();
                return await RedisClient.cache.StringGetAsync(key);
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetRedisCache - {e.Message}", nameof(GetRedisCache), exception: e);
                return null;
            }
        }


        public async Task<string> DeleteRedisCache(string key)
        {
            try
            {
                redisClient.ConnectRedisCache();
                await RedisClient.cache.KeyDeleteAsync(key);
                return Constants.Redis.OperationStatus.Success;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method DeleteRedisCache - {e.Message}", nameof(DeleteRedisCache), exception: e);
                return Constants.Redis.OperationStatus.Failure;
            }
        }
    }
}
