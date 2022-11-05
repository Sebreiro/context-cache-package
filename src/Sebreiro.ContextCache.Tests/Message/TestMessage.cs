using System;

namespace Sebreiro.ContextCache.Tests.Message
{
    public class TestMessage
    {
        public Guid Id { get; set; }
        public TestOne TestOne { get; set; }
        public TestTwo TestTwo { get; set; }
    }
}