using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TestProject.WebAPI;
using TestProject.WebAPI.Data;

namespace TestProject.Tests
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly TestProjectDbContext _dbContext;
        protected readonly CustomWebApplicationFactory<Startup> _factory;
        protected IServiceScope _scope;

        protected BaseTest(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            //create scope, because we need access to server db instance
            _scope = _factory.Server.Services.CreateScope();
            var scopedServices = _scope.ServiceProvider;
            _dbContext = scopedServices.GetRequiredService<TestProjectDbContext>();

            //clear db each test
            try
            {
                _dbContext.Database.EnsureDeleted();

            }
            catch { }
            finally
            {
                _dbContext.Database.EnsureCreated();
            }
            SeedTestProject();
        }

        protected void ReloadDbEntites()
        {
            foreach (var entity in _dbContext.ChangeTracker.Entries().ToList())
            {
                entity.Reload();
            }
        }

        private void SeedTestProject()
        {

        }

        public void Dispose()
        {

            _scope.Dispose();
            _client.Dispose();
        }
    }
}