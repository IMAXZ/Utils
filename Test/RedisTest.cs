using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Redis.StackExchange;

namespace Test
{
    class RedisTest
    {
        static void Main(string[] args)
        {
            var set = RedisUtils.Set("Test", "100");
            var get = RedisUtils.Get("Test");
            var add = RedisUtils.Add("Test", "101");
            get = RedisUtils.Get("Test");
            set = RedisUtils.Set("Test", "102");
            get = RedisUtils.Get("Test");
            var remove = RedisUtils.Remove("Test");
            set = RedisUtils.Set("Test", "test");
            set = RedisUtils.Set("Test1", "test", 60 * 60);
            var keytime = RedisUtils.KeyTimeToLive("Test1");
            set = RedisUtils.Set("int", "1");
            var increment = RedisUtils.Increment("int");
            var decrement = RedisUtils.Decrement("int", 2);
            var getset = RedisUtils.GetSet("int", "100");
            Console.ReadKey();
        }
    }
}
