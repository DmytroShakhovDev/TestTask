using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject.Tests
{
    public class CustomWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .UseEnvironment("IntegrationTests");
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureTestServices(services =>
            {
                //remove hosted services from tests
                services.Where(descriptor => descriptor.ServiceType == typeof(IHostedService)).ToList().ForEach(x => services.Remove(x));

            });
        }
    }
}
