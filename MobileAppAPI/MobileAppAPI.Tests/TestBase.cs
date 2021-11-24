using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Net.Http;

namespace MobileAppAPI.Tests
{
    public abstract class TestBase
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public TestBase()
        {
            Server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        builder
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables();
                    })
                .UseStartup<Startup>());
            Server.Host.Start();
            Client = Server.CreateClient();
        }
    }
}