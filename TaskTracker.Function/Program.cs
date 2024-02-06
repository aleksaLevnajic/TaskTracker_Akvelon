using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(TaskTracker.Function.Program))]

namespace TaskTracker.Function
{
    public class Program : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(InitializeCosmosClient);
        }

        private CosmosClient InitializeCosmosClient(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("AccountEndpoint"), Environment.GetEnvironmentVariable("AccountKey"));

            return cosmosClient;
        }
    }
}
