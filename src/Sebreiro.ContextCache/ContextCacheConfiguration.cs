using Microsoft.Extensions.DependencyInjection;
using System;
using Sebreiro.ContextCache.Options;

namespace Sebreiro.ContextCache
{
    /// <summary>
    /// Конфигурация запоминания контекста обработки
    /// </summary>
    public static class ContextCacheConfiguration
    {
        /// <summary>
        /// Период устаревания контекста обработки
        /// </summary>
        public static TimeSpan ExpirePeriod { get; private set; }

        /// <summary>
        /// Регистрация запоминания контекста обработки
        /// </summary>
        public static IServiceCollection AddContextCacheOptions(this IServiceCollection collection,
            string redisConfiguration, Action<ContextCacheOptions> action = null)
        {
            var contextCacheOptions = new ContextCacheOptions();
            action?.Invoke(contextCacheOptions);
            ExpirePeriod = TimeSpan.FromSeconds(contextCacheOptions.ContextExpireSeconds);
            collection.AddScoped<IContextCache, ContextCache>();
            return collection;
        }
    }
}