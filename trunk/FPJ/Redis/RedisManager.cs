using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FPJ.Context;

namespace Redis
{
    public static class RedisManager
    {
        private const string CacheKeyPrefix = "FPJ:";

        private static string GetFullCacheKey(string key)
        {
            return string.Concat(CacheKeyPrefix, key);
        }

        /// <summary>
        /// 设置key过期
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool KeyExpire(string key, TimeSpan? expiry)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.KeyExpire((RedisKey)key, expiry);
        }

        /// <summary>
        /// 删除key
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public static bool KeyDelete(string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.KeyDelete((RedisKey)key);
        }

        #region string opt

        /// <summary>
        /// 缓存key是否存在
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.KeyExists((RedisKey)key, 0);
        }

        /// <summary>
        /// 设置key的值value
        ///</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key, object value)
        {
            return Set(key, value, null);
        }
        /// <summary>
        /// 设置key的值，缓存时间
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiry">超时时间</param>
        /// <returns></returns>
        public static bool Set(string key, object value, TimeSpan? expiry)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringSet((RedisKey)key, (RedisValue)ToJson(value), expiry, 0, 0);
        }

        /// <summary>
        /// 异步设置key的值value
        ///</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<bool>
          SetAsync(string key, object value)
        {
            return SetAsync(key, value, null);
        }
        /// <summary>
        /// 异步设置缓存key的值
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static Task<bool>
          SetAsync(string key, object value, TimeSpan? expiry)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringSetAsync((RedisKey)key, (RedisValue)ToJson(value), expiry, 0, 0);
        }

        /// <summary>
        /// 自增缓存key的值（整数类型）
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（整数类型）</param>
        /// <returns>自增后返回的数值（为整数）</returns>
        public static long StringIncrement(string key, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringIncrement((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 自减缓存key的值（整数类型）
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（整数类型）</param>
        /// <returns>自减后返回的数值（整数类型）</returns>
        public static long StringDecrement(string key, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringDecrement((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 自增缓存key的值（整数类型）
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（浮点类型）</param>
        /// <returns>自增后返回的数值（为浮点类型）</returns>
        public static double StringIncrement(string key, double value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringIncrement((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 自减缓存key的值
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（浮点类型）</param>
        /// <returns>自减后返回的数值（为浮点类型）</returns>
        public static double StringDecrement(string key, double value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringDecrement((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 异步自增缓存key的值（整数类型）
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（整数类型）</param>
        /// <returns>自增后返回的数值（为整数）</returns>
        public static Task<long>
          StringIncrementAsync(string key, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringIncrementAsync((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 异步自减缓存key的值（整数类型）
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（整数类型）</param>
        /// <returns>自减后返回的数值（整数类型）</returns>
        public static Task<long>
          StringDecrementAsync(string key, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringDecrementAsync((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 异步自增缓存key的值（浮点类型）
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（浮点类型）</param>
        /// <returns>自增后返回的数值（为浮点类型）</returns>
        public static Task<double>
          StringIncrementAsync(string key, double value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringIncrementAsync((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 异步自减缓存key的值
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值（浮点类型）</param>
        /// <returns>自减后返回的数值（为浮点类型）</returns>
        public static Task<double>
          StringDecrementAsync(string key, double value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.StringDecrementAsync((RedisKey)key, value, 0);
        }

        /// <summary>
        /// 获取key的元素
        ///</summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T Get<T>
          (string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.StringGet((RedisKey)key, 0);

            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        /// <summary>
        /// 获取key的元素，如果不存在则只需func委托再返回数据
        ///</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="func">获取数据的委托函数</param>
        /// <param name="expiry">过期时间，不填默认为永久</param>
        /// <returns></returns>
        public static T Get<T> (string key, Func<T>    func, TimeSpan? expiry) where T : class
        {
            if (func == null)
                return default(T);

            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.StringGet((RedisKey)key, 0);
            var cacheContent = result.HasValue ? FromJson<T>
              (result) : default(T);

            if (cacheContent == null)
            {
                cacheContent = func();
                if (cacheContent != null)
                {
                    db.StringSet((RedisKey)key, (RedisValue)ToJson(cacheContent), expiry, 0, 0);
                }
            }
            return cacheContent;
        }

        public static T GetSet<T> (string key, object value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();

            var result = db.StringGetSet((RedisKey)key, (RedisValue)ToJson(value), 0);

            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        public static object GetSet(string key, object value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();

            var result = db.StringGetSet((RedisKey)key, (RedisValue)ToJson(value), 0);

            return result.HasValue ? FromJson<object>
              (result) : null;
        }


        #endregion

        #region SetAdd操作

        public static bool SetAdd<T> (string key, T value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.SetAdd((RedisKey)key, (RedisValue)ToJson(value), 0);
        }

        public static long SetAdd<T> (string key, IEnumerable<T> values)
        {
            if (values == null || values.Count() == 0)
            {
                return -1;
            }

            long length = 0;
            foreach (var item in values)
            {
                if (SetAdd(key, item))
                {
                    length++;
                }
            }

            return length;
        }

        public static bool SetContains(string key, object value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.SetContains((RedisKey)key, (RedisValue)ToJson(value), 0);
        }

        public static bool SetRemove<T> (string key, T value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.SetRemove((RedisKey)key, (RedisValue)ToJson(value), 0);
        }


        public static bool SetMove<T> (string source, string destination, T value)
        {
            var db = RedisDatabase.GetDatabase();
            return db.SetMove((RedisKey)source, (RedisKey)destination, (RedisValue)ToJson(value), 0);
        }

        #endregion

        #region hash opt

        /// <summary>
        /// hash中的key的hashname是否存在
        /// </summary>
        /// <param name="key">hash缓存key</param>
        /// <param name="hashName">hash缓存字段</param>
        /// <returns></returns>
        public static bool HashExists(string key, string hashName)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashExists((RedisKey)key, (RedisValue)hashName, 0);
        }

        /// <summary>
        /// 获取hash的key中字段hashName的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static T HashGet<T>  (string key, string hashName)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.HashGet((RedisKey)key, (RedisValue)hashName, 0);

            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        /// <summary>
        /// hash的值类型委托存储（针对值类型）
        ///</summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash字段</param>
        /// <param name="func">获取数据的委托方法</param>
        /// <returns></returns>
        public static T HashGetValueType<T>    (string key, string hashName, Func<T> func) where T : struct
        {
            if (func == null)
                return default(T);

            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();

            T value = default(T);
            if (!RedisManager.HashExists(key, hashName))
            {
                value = func();
                db.HashSet((RedisKey)key, (RedisValue)hashName, (RedisValue)ToJson(value), 0, 0);
            }
            else
            {
                var result = db.HashGet((RedisKey)key, (RedisValue)hashName, 0);
                value = result.HasValue ? FromJson<T>
                  (result) : default(T);
            }

            return value;
        }

        /// <summary>
        /// hash的引用类型委托存储（针对引用类型）
        /// </summary>
        /// <typeparam name="T">引用类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash字段</param>
        /// <param name="func">获取数据的委托方法</param>
        /// <returns></returns>
        public static T HashGet<T>  (string key, string hashName, Func<T>  func) where T : class
        {
            if (func == null)
                return default(T);

            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.HashGet((RedisKey)key, (RedisValue)hashName, 0);

            var cacheContent = result.HasValue ? FromJson<T>
              (result) : default(T);

            if (cacheContent == null)
            {
                cacheContent = func();
                if (cacheContent != null)
                {
                    db.HashSet((RedisKey)key, (RedisValue)hashName, (RedisValue)ToJson(cacheContent), 0, 0);
                }
            }

            return cacheContent;
        }

        /// <summary>
        /// 获取hash中key的所有值集合
        ///</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IDictionary<string, T> HashGetAll<T>(string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            IDictionary<string, T>
                dic = new Dictionary<string, T>
                  ();
            foreach (HashEntry entry in db.HashGetAll((RedisKey)key, 0))
            {
                RedisValue name = entry.Name;
                dic.Add(name, entry.Value.HasValue ? FromJson<T>
                  (entry.Value) : default(T));
            }

            return dic;
        }
        /// <summary>
        /// 获取hash中key的所有值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> HashGetListAll<T>   (string key)
        {
            var dicList = HashGetAll<T>
              (key);
            if (dicList == null)
            {
                return null;
            }

            return dicList.Values.ToList();
        }

        /// <summary>
        /// hash设置key中filed的值
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash字段</param>
        /// <param name="hashValue">缓存值</param>
        /// <returns></returns>
        public static bool HashSet(string key, string hashName, object hashValue)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashSet((RedisKey)key, (RedisValue)hashName, (RedisValue)ToJson(hashValue), 0, 0);
        }

        /// <summary>
        /// 异步hash设置key中filed的值
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash字段</param>
        /// <param name="hashValue">缓存值</param>
        /// <returns></returns>
        public static Task<bool>  HashSetAsync(string key, string hashName, object hashValue)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashSetAsync((RedisKey)key, (RedisValue)hashName, (RedisValue)ToJson(hashValue), 0, 0);
        }

        /// <summary>
        /// 删除hash中key的某个字段hashName
        ///    </summary>
        /// <param name="key"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static bool HashDelete(string key, string hashName)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashDelete((RedisKey)key, (RedisValue)hashName, 0);
        }

        /// <summary>
        /// 自增key对应的filed中的值，不存在则创建默认value
        ///  </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为整数）</param>
        /// <returns>返回自增后结果值（整数）</returns>
        public static long HashIncrement(string key, string hashName, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashIncrement((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 自减key对应的filed中的值，不存在或值类型不为整数则设置为0
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为整数）</param>
        /// <returns>返回自减后结果值（整数）</returns>
        public static long HashDecrement(string key, string hashName, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashDecrement((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 自增key对应的filed中的值，不存在则创建默认value
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为浮点数）</param>
        /// <returns>返回自增后结果值（浮点数）</returns>
        public static double HashIncrement(string key, string hashName, double value = 1.0)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashIncrement((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 自减key对应的filed中的值，不存在或值类型不为整数则设置为0
        ///  </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为浮点数）</param>
        /// <returns>返回自减后结果值（浮点数）</returns>
        public static double HashDecrement(string key, string hashName, double value = 1.0)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashDecrement((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 异步自增key对应的filed中的值，不存在则创建默认value
        ///  </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为整数）</param>
        /// <returns>返回自增后结果值（整数）</returns>
        public static Task<long>
          HashIncrementAsync(string key, string hashName, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashIncrementAsync((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 异步自减key对应的filed中的值，不存在或值类型不为整数则设置为0
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为整数）</param>
        /// <returns>返回自减后结果值（整数）</returns>
        public static Task<long>  HashDecrementAsync(string key, string hashName, long value = 1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashDecrementAsync((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 自增key对应的filed中的值，不存在则创建默认value
        ///   </summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为浮点数）</param>
        /// <returns>返回自增后结果值（浮点数）</returns>
        public static Task<double>  HashIncrementAsync(string key, string hashName, double value = 1.0)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashIncrementAsync((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        /// <summary>
        /// 异步自减key对应的filed中的值，不存在或值类型不为整数则设置为0
        ///</summary>
        /// <param name="key">缓存key</param>
        /// <param name="hashName">hash缓存对应filed</param>
        /// <param name="value">缓存值（必须为浮点数）</param>
        /// <returns>返回自减后结果值（浮点数）</returns>
        public static Task<double>  HashDecrementAsync(string key, string hashName, double value = 1.0)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.HashDecrementAsync((RedisKey)key, (RedisValue)hashName, value, 0);
        }

        #endregion

        #region list opt

        /// <summary>
        /// 插入单条数据到list表头（左侧）
        ///  </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">数据</param>
        /// <returns>返回插入成功条数</returns>
        public static long ListLeftPush<T>  (string key, T value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.ListLeftPush((RedisKey)key, (RedisValue)ToJson(value));
        }

        /// <summary>
        /// 插入多条数据到list表头（左侧）
        ///    </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">数据</param>
        /// <returns>返回插入成功条数</returns>
        public static long ListLeftPush<T>    (string key, IEnumerable<T>
            values)
        {
            if (values == null || values.Count() == 0)
            {
                return -1;
            }

            long length = 0;
            foreach (var item in values)
            {
                length = ListLeftPush(key, item);
            }

            return length;
        }

        /// <summary>
        /// 插入单条数据到list表尾（右侧）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">数据</param>
        /// <returns>返回插入成功条数</returns>
        public static long ListRightPush<T>   (string key, T value)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            return db.ListRightPush((RedisKey)key, (RedisValue)ToJson(value));
        }

        /// <summary>
        /// 插入多条数据到list表尾（右侧）
        ///  </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">数据</param>
        /// <returns>返回插入成功条数</returns>
        public static long ListRightPush<T>  (string key, IEnumerable<T>  values)
        {
            if (values == null || values.Count() == 0)
            {
                return -1;
            }

            long length = 0;
            foreach (var item in values)
            {
                length = ListRightPush(key, item);
            }

            return length;
        }

        /// <summary>
        /// 移除并返回列表key的头元素
        ///  </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T ListLeftPop<T>  (string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.ListLeftPop((RedisKey)key);
            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        /// <summary>
        /// 移除并返回列表key的尾元素
        ///    </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T ListRightPop<T> (string key)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.ListRightPop((RedisKey)key);
            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        /// <summary>
        /// 返回列表key制定区间的元素
        ///   </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="start">起始位置（0为第一个元素；使用负数，-1为最后一个元素，依次类推）</param>
        /// <param name="stop">结束位置（0为第一个元素；使用负数，-1为最后一个元素，依次类推）</param>
        /// <returns></returns>
        public static List<T>  ListRange<T>   (string key, long start = 0, long stop = -1)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.ListRange((RedisKey)key, start, stop);
            List<T>
              list = new List<T>
                ();
            foreach (var item in result)
            {
                list.Add(item.HasValue ? FromJson<T>
                  (item) : default(T));
            }

            return list;
        }

        /// <summary>
        /// 返回列表key中，下标为index元素
        ///   </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key"></param>
        /// <param name="index">下标（0为第一个元素；使用负数，-1为最后一个元素，依次类推）</param>
        /// <returns></returns>
        public static T ListGetByIndex<T>   (string key, long index)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            var result = db.ListGetByIndex((RedisKey)key, index);
            return result.HasValue ? FromJson<T>
              (result) : default(T);
        }

        /// <summary>
        /// 对列表进行修剪，只保留指定区间的元素，不在指定区间的元素都被删除
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start">起始位置（0为第一个元素；使用负数，-1为最后一个元素，依次类推）</param>
        /// <param name="stop">结束位置（0为第一个元素；使用负数，-1为最后一个元素，依次类推）</param>
        public static void ListTrim(string key, long start, long stop)
        {
            key = GetFullCacheKey(key);
            var db = RedisDatabase.GetDatabase();
            db.ListTrim((RedisKey)key, start, stop);
        }

        #endregion

        #region  当作消息代理中间件使用 一般使用更专业的消息队列来处理这种业务场景

        /// <summary>
        /// 当作消息代理中间件使用
        /// 消息组建中,重要的概念便是生产者,消费者,消息中间件。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(string channel, string message)
        {
            ISubscriber sub = RedisDatabase.Instance().GetSubscriber();
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 在消费者端处理消息
        /// </summary>
        /// <param name="channelFrom">消息频道</param>
        /// <param name="action">处理消息的动作</param>
        public static void Subscribe(string channelFrom, Action<object> action)
        {
            ISubscriber sub = RedisDatabase.Instance().GetSubscriber();
            sub.Subscribe(channelFrom, (channel, message) =>
            {
                action(message);
            });
        }

        #endregion


        private static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private static T FromJson<T>
          (string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class RedisDatabase
    {
        private static ConnectionMultiplexer _redis;

        private static readonly object SyncLock = new object();

        public static ConnectionMultiplexer Instance()
        {
            return _redis;
        }

        public static IDatabase GetDatabase()
        {
            if (_redis == null || !_redis.IsConnected || !_redis.GetDatabase().IsConnected(default(RedisKey)))
            {
                lock (SyncLock)
                {
                    try
                    {
                        var ops = ConfigurationOptions.Parse(Config.Instance.RedisConnectionStringConfig);
                        _redis = ConnectionMultiplexer.Connect(ops);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return _redis.GetDatabase();
        }

        public static void Dispose()
        {
            _redis.Dispose();
        }
    }
}
