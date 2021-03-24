using CSRedis;

namespace Pi.Common.Redis
{
    public static class RedisServer
    {
        public static CSRedisClient Cache;
        public static CSRedisClient Sequence;
        public static CSRedisClient Session;

        public static void Initalize()
        {
            Cache = new CSRedisClient(AppSettings.Configuration["RedisServer:Cache"]);
            Sequence = new CSRedisClient(AppSettings.Configuration["RedisServer:Sequence"]);
            Session = new CSRedisClient(AppSettings.Configuration["RedisServer:Session"]);
        }
    }
}
