using System.Threading.Tasks;
using Sebreiro.ContextCache.Tests.Message;
using Sebreiro.ContextCache.Tests.Mock;

namespace Sebreiro.ContextCache.Tests.Services
{
    public class TestServiceThree : IProcessor
    {
        private readonly IContextCache _context;
        private readonly TestWriter _writer;

        public TestServiceThree(IContextCache context, TestWriter writer)
        {
            _context = context;
            _writer = writer;
        }

        public async Task Run(TestMessage message)
        {
            _writer.Write($"Three:{message.TestTwo?.Test} start");
            await _context.ApplyAsync(() => _writer.Write($"Three:{message.TestTwo.Test}"));
            _writer.Write($"Three:{message.TestTwo?.Test} finish");
        }
    }
}