using System.Threading.Tasks;
using Sebreiro.ContextCache.Tests.Message;

namespace Sebreiro.ContextCache.Tests.Services
{
    public interface IProcessor
    {
        Task Run(TestMessage message);
    }
}