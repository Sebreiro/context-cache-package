using System.Threading.Tasks;
using Sebreiro.ContextCache.Tests.Message;
using Sebreiro.ContextCache.Tests.Mock;

namespace Sebreiro.ContextCache.Tests.Services
{
    public class TestServiceTwo : IProcessor
    {
        private readonly IContextCache _context;
        private readonly TestWriter _writer;

        public TestServiceTwo(IContextCache context, TestWriter writer)
        {
            _context = context;
            _writer = writer;
        }

        public async Task Run(TestMessage message)
        {
            _writer.Write($"Two:{message.TestOne?.Test} start");
            await _context.ApplyAsync(() => _writer.Write($"Two:{message.TestOne.Test}"));
            _writer.Write($"Two:{message.TestOne?.Test} finish");
        }
    }
}