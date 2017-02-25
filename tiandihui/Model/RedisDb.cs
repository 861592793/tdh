using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Model
{
    public class RedisDb
    {
        private static string[] aliReadWriteHosts = new[] {
                 "redis://Hewenlong123@123.56.72.214:6379"/*redis://密码@访问地址:端口*/
                 };
        PooledRedisClientManager redisPoolManager = new PooledRedisClientManager(4500/*连接池个数*/, 5/*连接池超时时间(秒)*/, aliReadWriteHosts);

        #region 写入List<T>泛型集合,默认DB0
        /// <summary>  
        /// 写入List<T>泛型类  
        /// </summary>  
        /// <typeparam name="T">泛型</typeparam>  
        /// <param name="keys">键</param>  
        /// <param name="value">泛型集合</param>  
        /// <param name="dt">到期时间</param>  
        /// <param name="db">其他数据盘</param> 
        /// <returns>bool 是否成功</returns>
        public bool Set<T>(string keys, List<T> value, int db = 0)
            where T : class
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Set<List<T>>(keys, value);
            }
        }
        public bool Set<T>(string keys, List<T> value, DateTime dt, int db = 0)
            where T : class
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Set<List<T>>(keys, value, dt);
            }
        }
        #endregion

        #region 读取List<T>泛型集合,默认DB0
        /// <summary>  
        /// 读取List<T>泛型集合,默认DB0
        /// </summary>  
        /// <typeparam name="T">泛型</typeparam>  
        /// <param name="keys">键</param>  
        /// <param name="db">其他数据盘</param> 
        /// <returns>泛型集合</returns>
        public List<T> Get<T>(string keys, int db = 0)
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetCacheClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Get<List<T>>(keys);
            }
        }
        #endregion

        #region 写入key/value,默认DB0
        /// <summary>  
        /// 写入key/value,默认DB0
        /// </summary>  
        /// <param name="keys">键</param>  
        /// <param name="value">值</param>  
        /// <param name="dt">到期时间</param>  
        /// <param name="db">其他数据盘</param>  
        /// <returns>bool 是否成功</returns>
        public bool Set(string keys, string value, int db = 0)
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Set(keys, value);
            }
        }
        public bool Set(string keys, string value, DateTime dt, int db = 0)
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Set(keys, value, dt);
            }
        }
        #endregion

        #region 读取key/value,默认DB0
        /// <summary>  
        /// 读取key/value,默认DB0
        /// </summary>  
        /// <param name="keys">键</param>  
        /// <param name="db">其他数据盘</param> 
        /// <returns>值</returns>
        public string Get(string keys, int db = 0)
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetReadOnlyClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.GetValue(keys);
            }
        }
        #endregion

        #region 删除指定key,默认DB0
        /// <summary>  
        /// 读取key/value,默认DB0
        /// </summary>  
        /// <param name="keys">键</param>  
        /// <param name="db">其他数据盘</param> 
        /// <returns>bool 是否成功</returns>
        public bool Del(string keys, int db = 0)
        {
            RedisConfig.VerifyMasterConnections = false;//需要设置
            using (var redisClient = redisPoolManager.GetClient())
            {
                if (db != 0)
                {
                    ((RedisClient)redisClient).ChangeDb(db);
                }
                RedisNativeClient redisNativeClient = (RedisNativeClient)redisClient;
                redisNativeClient.Client = null;//ApsaraDB for Redis不支持client setname所以这里需要显示的把client对象置为null
                return redisClient.Remove(keys);
            }
        }
        #endregion
    }
}
