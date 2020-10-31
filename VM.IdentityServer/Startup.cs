using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VM.Core.Entities;
using VM.Core.Interfaces;
using VM.Data.Repository;
using VM.Ident.Identity;
using VM.Identity.Store;
using VM.IdentityServer.Identity;

namespace VM.IdentityServer
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();

            services.AddIdentity<VmUser, VmUserRole>();
            services.AddScoped<IUserStore<VmUser>, IdentityUserStore>();
            services.AddScoped<IRoleStore<VmUserRole>, IdentityRoleStore>();



            services.AddIdentityServer()
               .AddDeveloperSigningCredential()
               .AddInMemoryIdentityResources(Config.Ids)
               .AddInMemoryClients(Config.Clients)
               .AddInMemoryApiResources(Config.Apis)
               .AddTestUsers(TestUsers.Users)
               .AddAspNetIdentity<VmUser>();

            CosmosClient cosmosClient = CreateCosmosClient();
            services.AddSingleton<CosmosClient>(e => cosmosClient);
            services.AddSingleton<IVmUserRepository, CosmosVmUserRepository>();

        }

        private CosmosClient CreateCosmosClient()
        {
            string connectionString = Configuration.GetConnectionString("CosmosConnection");
            return new CosmosClient(connectionString);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
