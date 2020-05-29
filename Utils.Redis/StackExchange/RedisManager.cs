using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Redis.StackExchange
{
    public class RedisManager
    {
        private static ConnectionMultiplexer _redis;
        private static readonly object locker = new object();
        public static ConnectionMultiplexer Conn
        {
            get
            {
                if (_redis != null && _redis.IsConnected)
                    return _redis;

                lock (locker)
                {
                    if (_redis != null && _redis.IsConnected) return _redis;
                    _redis = CreateManager();
                }
                return _redis;
            }
        }

        private static ConnectionMultiplexer CreateManager()
        {

            ConfigurationOptions config = new ConfigurationOptions
            {
                //stackexchange 解决奇怪的超时问题
                ConnectTimeout = 15000,
                SyncTimeout = 15000,
                ResponseTimeout = 15000,
                KeepAlive = 180,
                AbortOnConnectFail = false,
                EndPoints =
                {
                    { ConfigurationManager.AppSettings["RedisHost"], Convert.ToInt32(ConfigurationManager.AppSettings["RedisPort"]) }
                },
                Password = ConfigurationManager.AppSettings["RedisPwd"]
            };
            return ConnectionMultiplexer.Connect(config);
        }


        public static void Dispose()
        {
            _redis.Dispose();
        }

        public static void Reconnect()
        {
            _redis = CreateManager();
        }
    }
}
