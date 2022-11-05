namespace Sebreiro.ContextCache.Options
{
    /// <summary>
    /// Настройки запоминания контекста обработки
    /// </summary>
    public class ContextCacheOptions
    {
        /// <summary>
        /// Время сброса запоминания контекста обработки
        /// </summary>
        public int ContextExpireSeconds { get; set; } = 900;
    }
}
