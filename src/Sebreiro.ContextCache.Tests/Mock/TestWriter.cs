using System;
using System.Collections.Generic;

namespace Sebreiro.ContextCache.Tests.Mock
{
    public class TestWriter : IDisposable
    {
        private List<string> _list;

        public TestWriter()
        {
            _list = new List<string>();
        }

        public void Write(string test)
        {
            _list.Add(test);
        }

        public List<string> List => _list;

        public void Dispose()
        {
            _list.Clear();
            _list = null;
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}