using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace TestProject.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        var env = builderContext.HostingEnvironment;
                        config
                            .AddJsonFile("appsettings.json", false, true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                            .AddEnvironmentVariables();
                    });
                });
    }
}
