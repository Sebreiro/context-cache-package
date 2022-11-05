using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sebreiro.ContextCache.Tests.Message;
using Sebreiro.ContextCache.Tests.Mock;

namespace Sebreiro.ContextCache.Tests.Services
{
    public class TestServiceOne
    {
        private readonly IContextCache _context;
        private readonly IEnumerable<IProcessor> _processors;
        private readonly TestWriter _writer;

        public TestServiceOne(IContextCache context, IEnumerable<IProcessor> processors, TestWriter writer)
        {
            _context = context;
            _processors = processors;
            _writer = writer;
        }

        public async Task Run(TestMessage message)
        {
            _writer.Write($"One:{message.Id} start");
            await _context.ApplyAsync(() => _writer.Write($"One:{message.Id}"));
            _writer.Write($"One:{message.Id} finish");
            var tasks = _processors.Select(x => x.Run(message)).ToArray();
            Task.WaitAll(tasks);
        }
    }
}