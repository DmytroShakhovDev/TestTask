using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using TestProject.WebAPI.Data;
using TestProject.WebAPI.Helpers;
using TestProject.WebAPI.Repositories;
using TestProject.WebAPI.SeedData;
using TestProject.WebAPI.Services;

namespace TestProject.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var migrationsAssembly = typeof(TestProjectDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            IServiceCollection v = services.AddDbContextPool<TestProjectDbContext>(options =>
                 options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            var assamblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(item => Assembly.Load(item)).ToList();
            assamblies.Add(Assembly.GetExecutingAssembly());
            services.AddMediatR(assamblies.ToArray());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<RabbitMQClient, RabbitMQClient>();

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddSwaggerGen(c => 
            { 
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Service", Version = "v1" }); 
            }); 
            

            }
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Seed.EnsureSeedData(serviceScope.ServiceProvider);
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseMiddleware<JwtMiddleware>();
            Log.Logger = new LoggerConfiguration()
                                                .MinimumLevel.Debug()
                                                .WriteTo.Console()
                                                .WriteTo.File("log.txt")
                                                .CreateLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
            InitializeDatabase(app);
        }
    }
}
