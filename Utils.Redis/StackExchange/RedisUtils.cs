using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Redis.StackExchange
{
    public static class RedisUtils
    {
        private static IDatabase Db = RedisManager.Conn.GetDatabase(int.Parse(ConfigurationManager.AppSettings["RedisDb"]));

        #region Key处理

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return Db.KeyDelete(key);
        }

        public static Task<bool> RemoveAsync(string key)
        {
            return Db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Remove(string[] keys)
        {
            var tasks = new Task<bool>[keys.Length];
            var removed = 0L;

            for (var i = 0; i < keys.Length; i++)
                tasks[i] = RemoveAsync(keys[i]);

            for (var i = 0; i < keys.Length; i++)
                if (Db.Wait(tasks[i]))
                    removed++;

            return removed;
        }

        public static async Task<long> RemoveAsync(string[] keys)
        {
            var removed = 0L;

            foreach (var t in keys)
            {
                if (await Db.KeyDeleteAsync(t))
                    removed++;
            }
            return removed;
        }

        /// <summary>
        /// 是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return Db.KeyExists(key);
        }

        public static Task<bool> ContainsAsync(string key)
        {
            return Db.KeyExistsAsync(key);
        }

        /// <summary>
        /// 设置key的过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string key, DateTime? expiry)
        {
            return Db.KeyExpire(key, expiry);
        }

        public static Task<bool> KeyExpireAsync(string key, DateTime? expiry)
        {
            return Db.KeyExpireAsync(key, expiry);
        }

        /// <summary>
        /// key的剩余过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TimeSpan? KeyTimeToLive(string key)
        {
            return Db.KeyTimeToLive(key);
        }

        public static Task<TimeSpan?> KeyTimeToLiveAsync(string key)
        {
            return Db.KeyTimeToLiveAsync(key);
        }

        /// <summary>
        /// 返回key的类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RedisType KeyType(string key)
        {
            return Db.KeyType(key);
        }

        public static async Task<RedisType> KeyTypeAsync(string key)
        {
            return (await Db.KeyTypeAsync(key));
        }

        #endregion

        #region 字符串

        #region Set

        /// <summary>
        /// 添加 k v 数据，存在覆盖，second <= 0 永不过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool Set(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan());
        }

        public static bool Set<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Set(key, JsonConvert.SerializeObject(t), seconds);
        }

        public static Task<bool> SetAsync(string key, string value, int seconds = 0)
        {
            return Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan());
        }

        public static Task<bool> SetAsync<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return SetAsync(key, JsonConvert.SerializeObject(t), seconds);
        }

        public static bool Set(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray());
        }

        public static Task<bool> SetAsync(IDictionary<string, string> kvs)
        {
            return Db.StringSetAsync(kvs.ToKeyValuePairArray());
        }

        #endregion

        #region Add

        /// <summary>
        /// 添加 k v 数据，存在则不添加，second <= 0 永不过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool Add(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public static bool Add<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Db.StringSet(key, JsonConvert.SerializeObject(t), seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public static Task<bool> AddAsync(string key, string value, int seconds = 0)
        {
            return Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan(), When.NotExists);
        }

        public static bool Add(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray(), When.NotExists);
        }

        public static Task<bool> AddAsync(IDictionary<string, string> kvs)
        {
            return Db.StringSetAsync(kvs.ToKeyValuePairArray(), When.NotExists);
        }

        #endregion

        #region Get

        /// <summary>
        /// 获取 k v 数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return Db.StringGet(key);
        }

        public static T Get<T>(string key) where T : class
        {
            return JsonConvert.DeserializeObject<T>(Get(key));
        }

        public static string[] Get(string[] keys)
        {
            return Db.StringGet(keys.ToRedisKeyArray()).ToStringArray();
        }

        public static async Task<string> GetAsync(string key)
        {
            return await Db.StringGetAsync(key);
        }

        public static async Task<string[]> GetAsync(string[] keys)
        {
            return (await Db.StringGetAsync(keys.ToRedisKeyArray())).ToStringArray();
        }

        #endregion

        #region Increment

        /// <summary>
        /// 按增量递增存储在键上的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Increment(string key, long value = 1)
        {
            return Db.StringIncrement(key, value);
        }

        public static Task<long> IncrementAsync(string key, long value = 1)
        {
            return Db.StringIncrementAsync(key, value);
        }

        #endregion

        #region Decrement

        /// <summary>
        /// 按增量递减存储在键上的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Decrement(string key, long value = 1)
        {
            return Db.StringDecrement(key, value);
        }

        public static Task<long> DecrementAsync(string key, long value = 1)
        {
            return Db.StringDecrementAsync(key, value);
        }

        #endregion

        #region GetSet

        /// <summary>
        /// 将key设置为value并返回存储在key中的旧值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetSet(string key, string value)
        {
            return Db.StringGetSet(key, value);
        }

        public static async Task<string> GetSetAsync(string key, string value)
        {
            return await Db.StringGetSetAsync(key, value);
        }

        #endregion

        #region GetOrAdd

        /// <summary>
        /// 获取key的值没有则add
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aquire"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string GetOrAdd(string key, Func<string> aquire, int seconds = 0)
        {
            var data = Get(key);
            if (string.IsNullOrEmpty(data))
            {
                data = aquire();
                if (!string.IsNullOrEmpty(data))
                    Add(key, data, seconds);
            }
            return data;
        }

        public static T GetOrAdd<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new()
        {
            var data = Get<T>(key);
            if (data == null)
            {
                data = aquire();
                if (data != null)
                    Add(key, data, seconds);
            }
            return data;
        }

        public static async Task<string> GetOrAddAsync(string key, Func<string> aquire, int seconds = 0)
        {
            var data = await Db.StringGetAsync(key);
            var value = data.HasValue ? (string)data : null;
            if (string.IsNullOrEmpty(value))
            {
                value = aquire();
                if (!string.IsNullOrEmpty(value))
                    await Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan(), When.NotExists, CommandFlags.FireAndForget);
            }
            return value;
        }

        #endregion

        #region GetOrSet

        /// <summary>
        /// 获取key的值没有则set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="aquire"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string GetOrSet(string key, Func<string> aquire, int seconds = 0)
        {
            var data = Get(key);
            if (string.IsNullOrEmpty(data))
            {
                data = aquire();
                if (!string.IsNullOrEmpty(data))
                    Set(key, data, seconds);
            }
            return data;
        }

        public static T GetOrSet<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new()
        {
            var data = Get<T>(key);
            if (data == null)
            {
                data = aquire();
                if (data != null)
                    Set(key, data, seconds);
            }
            return data;
        }

        public static async Task<string> GetOrSetAsync(string key, Func<string> aquire, int seconds = 0)
        {
            var data = await Db.StringGetAsync(key);
            var value = data.HasValue ? (string)data : null;
            if (string.IsNullOrEmpty(value))
            {
                value = aquire();
                if (!string.IsNullOrEmpty(value))
                    await Db.StringSetAsync(key, value, seconds.ToRedisTimeSpan());
            }
            return value;
        }

        #endregion

        #endregion

        #region Hash

        #region HashSet

        /// <summary>
        /// 添加hash结构
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet(string key, string field, string value)
        {
            return Db.HashSet(key, field, value);
        }

        public static Task<bool> HashSetAsync(string key, string field, string value)
        {
            return Db.HashSetAsync(key, field, value);
        }

        /// <summary>
        /// 批量添加hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dic"></param>
        public static void HashSetBatch(string key, Dictionary<string, string> dic)
        {
            IBatch batch = Db.CreateBatch();
            foreach (var item in dic)
            {
                batch.HashSetAsync(key, item.Key, item.Value);
            }
            batch.Execute();
        }

        #endregion

        #region HashIncrement

        /// <summary>
        /// 按增量递增存储在hash键上的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long HashIncrement(string key, string field, long value = 1)
        {
            return Db.HashIncrement(key, field, value);
        }

        public static Task<long> HashIncrementAsync(string key, string field, long value = 1)
        {
            return Db.HashIncrementAsync(key, field, value);
        }

        #endregion

        #region HashDecrement

        /// <summary>
        /// 按增量递减存储在hash键上的数字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long HashDecrement(string key, string field, long value = 1)
        {
            return Db.HashDecrement(key, field, value);
        }

        public static Task<long> HashDecrementAsync(string key, string field, long value = 1)
        {
            return Db.HashDecrementAsync(key, field, value);
        }

        #endregion

        #region HashExists

        /// <summary>
        /// hash键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool HashExists(string key, string field)
        {
            return Db.HashExists(key, field);
        }

        public static Task<bool> HashExistsAsync(string key, string field)
        {
            return Db.HashExistsAsync(key, field);
        }

        #endregion

        #region HashDelete

        /// <summary>
        /// hash键删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string field)
        {
            return Db.HashDelete(key, field);
        }

        public static Task<bool> HashDeleteAsync(string key, string field)
        {
            return Db.HashDeleteAsync(key, field);
        }

        public static long HashDelete(string key, string[] fields)
        {
            return Db.HashDelete(key, fields.ToRedisValueArray());
        }

        public static Task<long> HashDeleteAsync(string key, string[] fields)
        {
            return Db.HashDeleteAsync(key, fields.ToRedisValueArray());
        }

        #endregion

        #region HashGet

        /// <summary>
        /// hash值获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string HashGet(string key, string field)
        {
            return Db.HashGet(key, field);
        }

        public static Task<RedisValue> HashGetAsync(string key, string field)
        {
            return Db.HashGetAsync(key, field);
        }

        #endregion

        #region HashGetAll

        /// <summary>
        /// 获取所有hash key以及对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string>[] HashGetAll(string key)
        {
            return Db.HashGetAll(key).ToHashPairs();
        }

        public static async Task<KeyValuePair<string, string>[]> HashGetAllAsync(string key)
        {
            return (await Db.HashGetAllAsync(key)).ToHashPairs();
        }

        #endregion

        #region HashKeys
        
        /// <summary>
        /// 获取所有hash key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] HashKeys(string key)
        {
            return Db.HashKeys(key).ToStringArray();
        }
        public static async Task<string[]> HashKeysAsync(string key)
        {
            return (await Db.HashKeysAsync(key)).ToStringArray();
        }

        #endregion

        #region HashValues

        /// <summary>
        /// 获取所有hash 值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] HashValues(string key)
        {
            return Db.HashValues(key).ToStringArray();
        }

        public static async Task<string[]> HashValuesAsync(string key)
        {
            return (await Db.HashValuesAsync(key)).ToStringArray();
        }

        #endregion

        #region HashLength

        /// <summary>
        /// 获取hash长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long HashLength(string key)
        {
            return Db.HashLength(key);
        }

        public static Task<long> HashLengthAsync(string key)
        {
            return Db.HashLengthAsync(key);
        }

        #endregion

        #region HashScan

        /// <summary>
        /// 分页查询hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        /// <param name="pageSize"></param>
        /// <param name="cursor"></param>
        /// <param name="pageOffset"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string>[] HashScan(string key, string pattern = null, int pageSize = 10, long cursor = 0, int pageOffset = 0)
        {
            return Db.HashScan(key, pattern, pageSize, cursor, pageOffset).ToHashPairs();
        }

        #endregion


        #endregion

        #region 列表

        #region ListLeftPush

        public static long ListLeftPush(string key, string value)
        {
            return Db.ListLeftPush(key, value);
        }

        public static long ListLeftPush(string key, string[] values)
        {
            return Db.ListLeftPush(key, values.ToRedisValueArray());
        }

        public static async Task<long> ListLeftPushAsync(string key, string value)
        {
            return await Db.ListLeftPushAsync(key, value);
        }

        public static async Task<long> ListLeftPushAsync(string key, string[] values)
        {
            return await Db.ListLeftPushAsync(key, values.ToRedisValueArray());
        }

        public static void ListLeftPushBatch(string key, List<string> list)
        {
            IBatch batch = Db.CreateBatch();
            foreach (var item in list)
            {
                batch.ListLeftPushAsync(key, item);
            }
            batch.Execute();
        }

        #endregion

        #region ListRightPush

        public static long ListRightPush(string key, string value)
        {
            return Db.ListRightPush(key, value);
        }
        public static long ListRightPush(string key, string[] values)
        {
            return Db.ListRightPush(key, values.ToRedisValueArray());
        }

        public static async Task<long> ListRightPushAsync(string key, string value)
        {
            return await Db.ListRightPushAsync(key, value);
        }

        public static async Task<long> ListRightPushAsync(string key, string[] values)
        {
            return await Db.ListRightPushAsync(key, values.ToRedisValueArray());
        }

        public static void ListRightPushBatch(string key, List<string> list)
        {
            IBatch batch = Db.CreateBatch();
            foreach (var item in list)
            {
                batch.ListRightPushAsync(key, item);
            }
            batch.Execute();
        }

        #endregion

        #region ListLeftPop

        public static string ListLeftPop(string key)
        {
            return Db.ListLeftPop(key);
        }

        public static async Task<string> ListLeftPopAsync(string key)
        {
            return await Db.ListLeftPopAsync(key);
        }

        #endregion

        #region ListRightPop

        public static string ListRightPop(string key)
        {
            return Db.ListRightPop(key);
        }

        public static async Task<string> ListRightPopAsync(string key)
        {
            return await Db.ListRightPopAsync(key);
        }

        #endregion

        #region ListLength

        public static long ListLength(string key)
        {
            return Db.ListLength(key);
        }

        public static Task<long> ListLengthAsync(string key)
        {
            return Db.ListLengthAsync(key);
        }

        #endregion

        #region ListRange

        public static string[] ListRange(string key, long start = 0, long stop = -1)
        {
            return Db.ListRange(key, start, stop).ToStringArray();
        }

        public static async Task<string[]> ListRangeAsync(string key, long start = 0, long stop = -1)
        {
            return (await Db.ListRangeAsync(key, start, stop)).ToStringArray();
        }

        #endregion

        #region ListRemove

        public static long ListRemove(string key, string value, long count = 0)
        {
            return Db.ListRemove(key, value, count);
        }

        public static Task<long> ListRemoveAsync(string key, string value, long count = 0)
        {
            return Db.ListRemoveAsync(key, value, count);
        }

        #endregion

        #region ListTrim

        public static void ListTrim(string key, long start, long stop)
        {
            Db.ListTrim(key, start, stop);
        }

        public static Task ListTrimAsync(string key, long start, long stop)
        {
            return Db.ListTrimAsync(key, start, stop);
        }

        #endregion

        #region ListInsertAfter

        public static long ListInsertAfter(string key, string pivot, string value)
        {
            return Db.ListInsertAfter(key, pivot, value);
        }

        public static Task<long> ListInsertAfterAsync(string key, string pivot, string value)
        {
            return Db.ListInsertAfterAsync(key, pivot, value);
        }

        #endregion

        #region ListInsertBefore

        public static long ListInsertBefore(string key, string pivot, string value)
        {
            return Db.ListInsertBefore(key, pivot, value);
        }

        public static Task<long> ListInsertBeforeAsync(string key, string pivot, string value)
        {
            return Db.ListInsertBeforeAsync(key, pivot, value);
        }

        #endregion

        #region ListGetByIndex

        public static string ListGetByIndex(string key, long index)
        {
            return Db.ListGetByIndex(key, index);
        }

        public static async Task<string> ListGetByIndexAsync(string key, long index)
        {
            return await Db.ListGetByIndexAsync(key, index);
        }

        #endregion

        #region ListSetByIndex

        public static void ListSetByIndex(string key, long index, string value)
        {
            Db.ListSetByIndex(key, index, value);
        }

        public static Task ListSetByIndexAsync(string key, long index, string value)
        {
            return Db.ListSetByIndexAsync(key, index, value);
        }

        #endregion

        #endregion

        #region 集合

        #region SetAdd

        public static bool SetAdd(string key, string value)
        {
            return Db.SetAdd(key, value);
        }

        public static long SetAdd(string key, string[] values)
        {
            return Db.SetAdd(key, values.ToRedisValueArray());
        }

        public static Task<bool> SetAddAsync(string key, string value)
        {
            return Db.SetAddAsync(key, value);
        }

        public static Task<long> SetAddAsync(string key, string[] values)
        {
            return Db.SetAddAsync(key, values.ToRedisValueArray());
        }

        #endregion

        #region SetCombine

        public static string[] SetCombine(SetOperation operation, string first, string second)
        {
            return Db.SetCombine(operation, first, second).ToStringArray();
        }
        public static async Task<string[]> SetCombineAsync(SetOperation operation, string first, string second)
        {
            return (await Db.SetCombineAsync(operation, first, second)).ToStringArray();
        }

        public static string[] SetCombine(SetOperation operation, string[] keys)
        {
            return Db.SetCombine(operation, keys.ToRedisKeyArray()).ToStringArray();
        }

        public static async Task<string[]> SetCombineAsync(SetOperation operation, string[] keys)
        {
            return (await Db.SetCombineAsync(operation, keys.ToRedisKeyArray())).ToStringArray();
        }

        #endregion

        #region SetCombineAndStore

        public static long SetCombineAndStore(SetOperation operation, string desctination, string first, string second)
        {
            return Db.SetCombineAndStore(operation, desctination, first, second);
        }

        public static Task<long> SetCombineAndStoreAsync(SetOperation operation, string desctination, string first, string second)
        {
            return Db.SetCombineAndStoreAsync(operation, desctination, first, second);
        }

        public static long SetCombineAndStore(SetOperation operation, string desctination, string[] keys)
        {
            return Db.SetCombineAndStore(operation, desctination, keys.ToRedisKeyArray());
        }

        public static Task<long> SetCombineAndStoreAsync(SetOperation operation, string desctination, string[] keys)
        {
            return Db.SetCombineAndStoreAsync(operation, desctination, keys.ToRedisKeyArray());
        }

        #endregion

        #region SetContains

        public static bool SetContains(string key, string value)
        {
            return Db.SetContains(key, value);
        }

        public static Task<bool> SetContainsAsync(string key, string value)
        {
            return Db.SetContainsAsync(key, value);
        }

        #endregion

        #region SetLength

        public static long SetLength(string key)
        {
            return Db.SetLength(key);
        }

        public static Task<long> SetLengthAsync(string key)
        {
            return Db.SetLengthAsync(key);
        }

        #endregion

        #region SetMembers

        public static string[] SetMembers(string key)
        {
            return Db.SetMembers(key).ToStringArray();
        }

        public static async Task<string[]> SetMembersAsync(string key)
        {
            return (await Db.SetMembersAsync(key)).ToStringArray();
        }

        #endregion

        #region SetMove

        public static bool SetMove(string source, string desctination, string value)
        {
            return Db.SetMove(source, desctination, value);
        }

        public static Task<bool> SetMoveAsync(string source, string desctination, string value)
        {
            return Db.SetMoveAsync(source, desctination, value);
        }

        #endregion

        #region SetPop

        public static string SetPop(string key)
        {
            return Db.SetPop(key);
        }

        public static async Task<string> SetPopAsync(string key)
        {
            return await Db.SetPopAsync(key);
        }

        #endregion

        #region SetRandomMember

        public static string SetRandomMember(string key)
        {
            return Db.SetRandomMember(key);
        }

        public static async Task<string> SetRandomMemberAsync(string key)
        {
            return await Db.SetRandomMemberAsync(key);
        }

        #endregion

        #region SetRandomMembers

        public static string[] SetRandomMembers(string key, long count)
        {
            return Db.SetRandomMembers(key, count).ToStringArray();
        }

        public static async Task<string[]> SetRandomMembersAsync(string key, long count)
        {
            return (await Db.SetRandomMembersAsync(key, count)).ToStringArray();
        }

        #endregion

        #region SetRemove

        public static bool SetRemove(string key, string value)
        {
            return Db.SetRemove(key, value);
        }

        public static Task<bool> SetRemoveAsync(string key, string value)
        {
            return Db.SetRemoveAsync(key, value);
        }

        public static long SetRemove(string key, string[] values)
        {
            return Db.SetRemove(key, values.ToRedisValueArray());
        }

        public static Task<long> SetRemoveAsync(string key, string[] values)
        {
            return Db.SetRemoveAsync(key, values.ToRedisValueArray());
        }

        #endregion

        #region SetScan

        public static string[] SetScan(string key, string pattern = null, int pageSize = 0, long cursor = 0, int pageOffset = 0)
        {
            return Db.SetScan(key, pattern, pageSize, cursor, pageOffset).ToArray().ToStringArray();
        }

        #endregion

        #endregion

        #region 有序集合

        #region SortedSetAdd

        public static bool SortedSetAdd(string key, string member, double score)
        {
            return Db.SortedSetAdd(key, member, score);
        }

        public static Task<bool> SortedSetAddAsync(string key, string member, double score)
        {
            return Db.SortedSetAddAsync(key, member, score);
        }

        public static long SortedSetAdd(string key, IDictionary<string, double> members)
        {
            return Db.SortedSetAdd(key, members.ToSortedSetEntry());
        }

        public static Task<long> SortedSetAddAsync(string key, IDictionary<string, double> members)
        {
            return Db.SortedSetAddAsync(key, members.ToSortedSetEntry());
        }

        #endregion

        #region SortedSetCombineAndStore

        public static long SortedSetCombineAndStore(SetOperation operation, string desctination, string first, string second, Aggregate aggregate = Aggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation, desctination, first, second, aggregate);
        }

        public static Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, string desctination, string first, string second, Aggregate aggregate = Aggregate.Sum)
        {
            return Db.SortedSetCombineAndStoreAsync(operation, desctination, first, second, aggregate);
        }

        public static long SortedSetCombineAndStore(SetOperation operation, string desctination, string[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation, desctination, keys.ToRedisKeyArray(), weights, aggregate);
        }

        public static Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, string desctination, string[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum)
        {
            return Db.SortedSetCombineAndStoreAsync(operation, desctination, keys.ToRedisKeyArray(), weights, aggregate);
        }

        #endregion

        #region SortedSetDecrement

        public static double SortedSetDecrement(string key, string member, double value)
        {
            return Db.SortedSetDecrement(key, member, value);
        }

        public static Task<double> SortedSetDecrementAsync(string key, string member, double value)
        {
            return Db.SortedSetDecrementAsync(key, member, value);
        }

        #endregion

        #region SortedSetIncrement

        public static double SortedSetIncrement(string key, string member, double value)
        {
            return Db.SortedSetIncrement(key, member, value);
        }

        public static Task<double> SortedSetIncrementAsync(string key, string member, double value)
        {
            return Db.SortedSetIncrementAsync(key, member, value);
        }

        #endregion

        #region SortedSetLength

        public static long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetLength(key, min, max, exclude);
        }

        public static Task<long> SortedSetLengthAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetLengthAsync(key, min, max, exclude);
        }

        #endregion

        #region SortedSetLengthByValue

        public static long SortedSetLengthByValue(string key, string min, string max, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetLengthByValue(key, min, max, exclude);
        }

        public static Task<long> SortedSetLengthByValueAsync(string key, string min, string max, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetLengthByValueAsync(key, min, max, exclude);
        }

        #endregion

        #region SortedSetRangeByRank

        public static string[] SortedSetRangeByRank(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return Db.SortedSetRangeByRank(key, start, stop, order).ToStringArray();
        }

        public static async Task<string[]> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return (await Db.SortedSetRangeByRankAsync(key, start, stop, order)).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByRankWithScores

        public static KeyValuePair<string, double>[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return Db.SortedSetRangeByRankWithScores(key, start, stop, order).ToSortedPairs();
        }

        public static async Task<KeyValuePair<string, double>[]> SortedSetRangeByRankWithScoresAsync(string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            return (await Db.SortedSetRangeByRankWithScoresAsync(key, start, stop, order)).ToSortedPairs();
        }

        #endregion

        #region SortedSetRangeByScore

        public static string[] SortedSetRangeByScore(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScore(key, start, stop, exclude, order, skip, take).ToStringArray();
        }

        public static async Task<string[]> SortedSetRangeByScoreAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByScoreAsync(key, start, stop, exclude, order, skip, take)).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByScoreWithScores

        public static KeyValuePair<string, double>[] SortedSetRangeByScoreWithScores(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScoreWithScores(key, start, stop, exclude, order, skip, take).ToSortedPairs();
        }

        public static async Task<KeyValuePair<string, double>[]> SortedSetRangeByScoreWithScoresAsync(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByScoreWithScoresAsync(key, start, stop, exclude, order, skip, take)).ToSortedPairs();
        }

        #endregion

        #region SortedSetRangeByValue

        public static string[] SortedSetRangeByValue(string key, string min = null, string max = null, Exclude exclude = Exclude.None, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByValue(key, min, max, exclude, skip, take).ToStringArray();
        }

        public static async Task<string[]> SortedSetRangeByValueAsync(string key, string min = null, string max = null, Exclude exclude = Exclude.None, long skip = 0, long take = -1)
        {
            return (await Db.SortedSetRangeByValueAsync(key, min, max, exclude, skip, take)).ToStringArray();
        }

        #endregion

        #region SortedSetRank

        public static long? SortedSetRank(string key, string member, Order order = Order.Ascending)
        {
            return Db.SortedSetRank(key, member, order);
        }

        public static Task<long?> SortedSetRankAsync(string key, string member, Order order = Order.Ascending)
        {
            return Db.SortedSetRankAsync(key, member, order);
        }

        #endregion

        #region SortedSetRemove

        public static bool SortedSetRemove(string key, string member)
        {
            return Db.SortedSetRemove(key, member);
        }

        public static long SortedSetRemove(string key, string[] members)
        {
            return Db.SortedSetRemove(key, members.ToRedisValueArray());
        }

        public static Task<bool> SortedSetRemoveAsync(string key, string member)
        {
            return Db.SortedSetRemoveAsync(key, member);
        }

        public static Task<long> SortedSetRemoveAsync(string key, string[] members)
        {
            return Db.SortedSetRemoveAsync(key, members.ToRedisValueArray());
        }

        #endregion

        #region SortedSetRemoveRangeByRank

        public static long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            return Db.SortedSetRemoveRangeByRank(key, start, stop);
        }

        public static Task<long> SortedSetRemoveRangeByRankAsync(string key, long start, long stop)
        {
            return Db.SortedSetRemoveRangeByRankAsync(key, start, stop);
        }

        #endregion

        #region SortedSetRemoveRangeByScore

        public static long SortedSetRemoveRangeByScore(string key, double start, double stop, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetRemoveRangeByScore(key, start, stop, exclude);
        }

        public static Task<long> SortedSetRemoveRangeByScoreAsync(string key, double start, double stop, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude);
        }

        #endregion

        #region SortedSetRemoveRangeByValue

        public static long SortedSetRemoveRangeByValue(string key, string min, string max, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetRemoveRangeByValue(key, min, max, exclude);
        }

        public static Task<long> SortedSetRemoveRangeByValueAsync(string key, string min, string max, Exclude exclude = Exclude.None)
        {
            return Db.SortedSetRemoveRangeByValueAsync(key, min, max, exclude);
        }

        #endregion

        #region SortedSetScan

        public static KeyValuePair<string, double>[] SortedSetScan(string key, string pattern = null, int pageSize = 10, int cursor = 0, int pageOffset = 0)
        {
            return Db.SortedSetScan(key, pattern, pageSize, cursor, pageOffset).ToSortedPairs();
        }

        #endregion

        #region SortedSetScore

        public static double? SortedSetScore(string key, string member)
        {
            return Db.SortedSetScore(key, member);
        }

        public static Task<double?> SortedSetScoreAsync(string key, string member)
        {
            return Db.SortedSetScoreAsync(key, member);
        }

        #endregion

        #endregion

    }
}
