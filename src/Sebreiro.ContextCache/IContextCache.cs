using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sebreiro.ContextCache
{
    /// <summary>
    /// Интерфейс запоминания контекста обработки
    /// </summary>
    public interface IContextCache
    {
        /// <summary>
        /// Создание контекста обработки
        /// </summary>
        /// <param name="message">Сообщение</param>
        void CreateContext<T>(T message) where T : class;

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        Task ApplyAsync(Expression<Action> action);

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        Task ApplyAsync(Expression<Func<Task>> action);

        /// <summary>
        /// Применение изменений в контексте обработки
        /// </summary>
        /// <param name="action">Изменение</param>
        void Apply(Expression<Action> action);
    }
}