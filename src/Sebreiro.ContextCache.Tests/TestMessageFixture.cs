using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sebreiro.ContextCache.Tests.Mock;
using Sebreiro.ContextCache.Tests.Services;
using StackExchange.Redis;

namespace Sebreiro.ContextCache.Tests
{
    public class TestMessageFixture : IDisposable
    {
        public TestMessageFixture()
        {
            var serviceCollection = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddOptions();
            serviceCollection.AddContextCacheOptions(config["RedisOptions:Configuration"]);
            AddRedisConnection(serviceCollection, config);
            serviceCollection.AddTransient<TestMessageHandler>();
            serviceCollection.AddScoped<TestWriter>();
            serviceCollection.AddScoped<TestServiceOne>();
            serviceCollection.AddScoped<IProcessor, TestServiceTwo>();
            serviceCollection.AddScoped<IProcessor, TestServiceThree>();

            Services = serviceCollection;
        }

        private void AddRedisConnection(ServiceCollection serviceCollection, IConfiguration config)
        {
            string configString = config["RedisOptions:Configuration"];
            var options = ConfigurationOptions.Parse(configString);
            var connection = ConnectionMultiplexer.Connect(options);
            serviceCollection.AddSingleton(connection);
        }

        public ServiceCollection Services { get; }

        public void Dispose()
        {
        }
    }
}