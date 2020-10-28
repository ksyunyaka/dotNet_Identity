using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using VM.Areas.Identity;
using VM.Core.Entities;
using VM.Data.Identity;
using VM.Core.Interfaces;
using VM.Data.Repository;
using VM.Web.Data;
using Microsoft.Azure.Cosmos;

namespace VM.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();

            // identity config
            services.AddIdentity<VmUser, VmUserRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            services.AddScoped<IUserStore<VmUser>, IdentityUserStore>();
            services.AddScoped<IRoleStore<VmUserRole>, IdentityRoleStore>();
                       
            //blazor config
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<VmUser>>();

            //app services config
            CosmosClient cosmosClient = CreateCosmosClient();
            services.AddSingleton<CosmosClient>(e => cosmosClient);
            services.AddSingleton<IVmUserRepository, CosmosVmUserRepository>();
            services.AddSingleton<WeatherForecastService>();

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
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
