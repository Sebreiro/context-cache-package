using System;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CachingFramework.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Sebreiro.ContextCache
{
    /// <summary>
    /// Контекст запоминания контекста обработки
    /// </summary>
    public sealed class ContextCache : IContextCache
    {
        private const string RedisCatalogName = "ContextCache";
        private readonly ConnectionMultiplexer _connection;
        private string _contextKey;
        private RedisContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ContextCache(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Создание контекста обработки
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void CreateContext<T>(T message) where T : class
        {
            if (message == null)
            {
                throw new Exception("Context to cache can not be initialized from null!");
            }

            var hashedMessage = GetHashString(JsonConvert.SerializeObject(message));
            _contextKey = GetFullKey(RedisCatalogName, hashedMessage);
            _context = new RedisContext(_connection);
        }

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        public async Task ApplyAsync(Expression<Action> action)
        {
            if (_contextKey == null)
            {
                throw new Exception("Context to cache not initialized!");
            }

            var actionKey = GetHashString(action.ToString());
            var list = _context.Collections.GetRedisList<string>(_contextKey);
            if (!(await list.ContainsAsync(actionKey)))
            {
                action.Compile().Invoke();
                await list.AddAsync(actionKey);
                list.TimeToLive = ContextCacheConfiguration.ExpirePeriod;
            }
        }

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        public async Task ApplyAsync(Expression<Func<Task>> action)
        {
            if (_contextKey == null)
            {
                throw new Exception("Context to cache not initialized!");
            }

            var actionKey = GetHashString(action.ToString());
            var list = _context.Collections.GetRedisList<string>(_contextKey);
            if (!(await list.ContainsAsync(actionKey)))
            {
                await action.Compile().Invoke();
                await list.AddAsync(actionKey);
                list.TimeToLive = ContextCacheConfiguration.ExpirePeriod;
            }
        }

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        public void Apply(Expression<Action> action)
        {
            if (_contextKey == null)
            {
                throw new Exception("Context to cache not initialized!");
            }

            var actionKey = GetHashString(action.ToString());
            var list = _context.Collections.GetRedisList<string>(_contextKey);
            if (!(list.Contains(actionKey)))
            {
                action.Compile().Invoke();
                list.Add(actionKey);
                list.TimeToLive = ContextCacheConfiguration.ExpirePeriod;
            }
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private string GetFullKey(string serviceName, string message)
        {
            return $"{serviceName}:{message}";
        }
    }
}