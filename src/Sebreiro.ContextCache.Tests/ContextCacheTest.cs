using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sebreiro.ContextCache.Tests.Message;
using Sebreiro.ContextCache.Tests.Mock;
using Sebreiro.ContextCache.Tests.Services;
using Shouldly;
using Xunit;

namespace Sebreiro.ContextCache.Tests
{
    public class ContextCacheTest : IClassFixture<TestMessageFixture>
    {
        private readonly ServiceProvider serviceProvider;

        public ContextCacheTest(TestMessageFixture fixture)
        {
            serviceProvider = fixture.Services.BuildServiceProvider();
        }

        [Fact]
        public async Task HandleMessage_Success()
        {
            //Arrange
            var handler = serviceProvider.GetService<TestMessageHandler>();
            var writer = serviceProvider.GetService<TestWriter>();
            var testMessage = new TestMessage
            {
                Id = Guid.NewGuid(),
                TestOne = new TestOne { Test = Guid.NewGuid() },
                TestTwo = new TestTwo { Test = Guid.NewGuid() },
            };

            //Action
            await handler.Run(testMessage);

            //Assert
            await Task.Delay(100);
            var list = writer.List;
            list.FirstOrDefault(x => x == $"One:{testMessage.Id}").ShouldNotBeNull();
            list.FirstOrDefault(x => x == $"Two:{testMessage.TestOne.Test}").ShouldNotBeNull();
            list.FirstOrDefault(x => x == $"Three:{testMessage.TestTwo.Test}").ShouldNotBeNull();
        }

        [Fact]
        public async Task HandleMessage_Repeat_Success()
        {
            //Arrange
            var handler = serviceProvider.GetService<TestMessageHandler>();
            var writer = serviceProvider.GetService<TestWriter>();
            var testMessage = new TestMessage
            {
                Id = Guid.NewGuid(),
                TestOne = new TestOne { Test = Guid.NewGuid() },
                TestTwo = new TestTwo { Test = Guid.NewGuid() },
            };
            await handler.Run(testMessage);
            await Task.Delay(100);
            writer.Clear();

            //Action
            await handler.Run(testMessage);

            //Assert
            await Task.Delay(100);
            var list = writer.List;
            list.Count.ShouldBe(6);
            list.FirstOrDefault(x => x == $"One:{testMessage.Id}").ShouldBeNull();
            list.FirstOrDefault(x => x == $"Two:{testMessage.TestOne.Test}").ShouldBeNull();
            list.FirstOrDefault(x => x == $"Three:{testMessage.TestTwo.Test}").ShouldBeNull();
        }

        [Fact]
        public async Task HandleMessage_WrongMessage_Failed()
        {
            //Arrange
            var handler = serviceProvider.GetService<TestMessageHandler>();
            var writer = serviceProvider.GetService<TestWriter>();
            var testMessage = new TestMessage
            {
                Id = Guid.NewGuid(),
                TestOne = new TestOne { Test = Guid.NewGuid() }
            };

            //Action
            var fail = false;
            try
            {
                await handler.Run(testMessage);
            }
            catch (Exception e)
            {
                fail = true;
            }

            //Assert
            fail.ShouldBeTrue();
            await Task.Delay(100);
            var list = writer.List;
            list.Count.ShouldBe(7);
            list.FirstOrDefault(x => x == $"One:{testMessage.Id}").ShouldNotBeNull();
            list.FirstOrDefault(x => x == $"Two:{testMessage.TestOne.Test}").ShouldNotBeNull();
            list.FirstOrDefault(x => x == $"Three:{testMessage.TestTwo?.Test}").ShouldBeNull();
        }
    }
}