using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; } // Provides access to appsettings.json file

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); // For MVC framework setup

            services.AddDbContextPool<StoreDbContext>(options => // The pool caches the instance of StoreDbContext, so it doesn't have to create everytime
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"]); // Add EF core db context service
            });

            services.AddScoped<IStoreRepository, EFStoreRepository>(); // Letting know that EFStoreRepositoy implements IStoreRepository. One more eg: InMemoryStoreRepository
            //AddScoped because we want Instance of EFStoreRepository to be alive through the entire scope of the HttpRequest.
            //New instance is created for every Http request, and lives theoughout the scope of that HttpRequest.
            services.AddScoped<IOrderRepository, EFOrderRepository>();
            services.AddRazorPages(); // sets up services used by razor pages

            //For session store
            services.AddDistributedMemoryCache(); //sets up the in-memory data store
            services.AddSession();//registers services used to access session data
            //Whereever a Cart object is used, use whatever object is returned from .GetCart method.
            //Scoped means use the same instance for all the related Http requests for Cart instances.
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp)); // sp as in service provider. Lambda expression will be invoked to satisfy Cart requests.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddServerSideBlazor();
            //For setting up authentication
            services.AddDbContext<AppIdentityDbContext>(options => {
                options.UseSqlServer(Configuration["ConnectionStrings:IdentityConnection"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseExceptionHandler("/error");
            }
            else // Put these 2 guys in here which were outside by default.
            {
                app.UseDeveloperExceptionPage(); // This is an extension method
                app.UseStatusCodePages();
            }

            app.UseStaticFiles();

            app.UseRouting(); // Remember with R-A-A-S-E

            //For session. IMPORTANT : This line must be above than the app.UseEndpoints
            app.UseSession(); //allows (the session system) to associate requests with sessions when they arrive from the client

            //For setting up authentication and authorization. This must be between .UseRouting and .UseEndpoints
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("catpage", "{category}/Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("page", "Page{productPage:int}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("category", "{category}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapControllerRoute("pagination", "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 });
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();//Registers Razor pages as endpoints that the URL routing system can use to handle requests

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");
            });

            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
