using System.Threading.Tasks;
using Sebreiro.ContextCache.Tests.Message;

namespace Sebreiro.ContextCache.Tests.Services
{
    public class TestMessageHandler
    {
        private readonly IContextCache _context;
        private readonly TestServiceOne _serviceOne;

        public TestMessageHandler(IContextCache context, TestServiceOne serviceOne)
        {
            _context = context;
            _serviceOne = serviceOne;
        }
        public async Task Run(TestMessage message)
        {
            _context.CreateContext(message);
            await _serviceOne.Run(message);
        }
    }
}