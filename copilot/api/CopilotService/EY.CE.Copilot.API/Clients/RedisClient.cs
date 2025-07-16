using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using StackExchange.Redis;

namespace EY.CE.Copilot.API.Clients
{
    public class RedisClient : IRedisClient
    {
        private readonly IConfiguration configuration;
        private static string cacheName, cachePassword, cachePort;
        public static StackExchange.Redis.IDatabase cache;
        private static string environment;
        int retriesLeft = 5;
        public RedisClient(IConfiguration config)
        {
            this.configuration = config;

            cacheName = config[SharedKeyVault.REDIS_NAME];

            cachePort = config[SharedKeyVault.REDIS_PORT];

            cachePassword = config[SharedKeyVault.REDIS_KEY];

            environment = config[SharedKeyVault.REDIS_ENVIRONMENT];
        }

        private static Lazy<ConnectionMultiplexer> GetLazyConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
            {
                if (!string.IsNullOrWhiteSpace(environment) && environment.ToLower() == Constants.DevelopmentEnvironment)
                {
                    Console.WriteLine($"Connecting to Redis Cache env {environment} cachename{cacheName} port {cachePort}");

                    var configurationOptions = new ConfigurationOptions
                    {
                        EndPoints = { cacheName, cachePort },
                        KeepAlive = 180,
                        Password = cachePassword,
                        ConnectTimeout = 10000,
                        // Needed for cache clear
                        AllowAdmin = true
                    };

                    var x = ConnectionMultiplexer.Connect(configurationOptions);
                    return x;
                }
                else
                {
                    Console.WriteLine($"Connecting to Redis Cache env {environment} cachename{cacheName} cachePassword {cachePassword} port {Constants.Redis.RedisSentinelPort} {Constants.Redis.RedisServiceName}");

                    var configurationOptions = new ConfigurationOptions
                    {

                        AbortOnConnectFail = false,
                        EndPoints = { cacheName, Constants.Redis.RedisSentinelPort },
                        Password = cachePassword,
                        ServiceName = Constants.Redis.RedisServiceName,
                        CommandMap = CommandMap.Sentinel,
                        TieBreaker = String.Empty,
                        AllowAdmin = true

                    };
                    var x = ConnectionMultiplexer.SentinelConnect(configurationOptions);

                    ConfigurationOptions masterConfig = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false,
                        ServiceName = Constants.Redis.RedisServiceName,
                        Password = cachePassword,
                        AllowAdmin = true
                    };

                    var masterConnection = x.GetSentinelMasterConnection(masterConfig);

                    return masterConnection;
                }
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection = GetLazyConnection();

        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (lazyConnection == null)
                    lazyConnection = GetLazyConnection();
                return lazyConnection.Value;
            }
        }

        public void ConnectRedisCache()
        {
            try
            {
                cache = Connection.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting bitnami redis" + ex.StackTrace);
                if(retriesLeft-- > 0)
                {
                    Console.WriteLine("Retrying redis connection : Retries left "+ retriesLeft);
                    ConnectRedisCache();
                }
                lazyConnection = null;
            }
        }
    }
}