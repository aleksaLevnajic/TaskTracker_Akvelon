using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.API;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTrackerAPi.IntefrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {       

        public IUnitOfWork UnitOfWork { get; set; }

        public CustomWebApplicationFactory()
        {
            UnitOfWork = Substitute.For<IUnitOfWork>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                /*services.AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });*/
                services.RemoveAll(typeof(DbContextOptions<TaskTrackerDbContext>));
                services.AddDbContext<TaskTrackerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                //services.AddSingleton(UnitOfWork);                
            });

            //builder.UseEnvironment("Development");
        }







        /*protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TaskTrackerDbContext>));

                var connString = GetConnectionString();
                services.AddSqlServer<TaskTrackerDbContext>(connString);
            });
        }
        private static string GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                                .AddUserSecrets<CustomWebApplicationFactory>()
                                .Build();
            var connectionString = configuration.GetConnectionString("TestingConnectionString")!;

            return connectionString;
        }*/
    }
}
