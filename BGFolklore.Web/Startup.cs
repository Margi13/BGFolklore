using BGFolklore.Data;
using BGFolklore.Data.Models;
using BGFolklore.Services.Public;
using BGFolklore.Services.Public.Interfaces;
using BGFolklore.Web.Common;
using BGFolklore.Web.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;

namespace BGFolklore.Web
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
            IConfiguration configCredentials = Configuration.GetSection("ConnectionStringsCredentials");
            string connectionString = string.Format(Configuration.GetConnectionString("DefaultConnection"), configCredentials["User"], configCredentials["Password"]);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization();

            //Makes custom default error message, when value is null. It's used when default value is empty string.
            services.AddRazorPages()
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        _ => "Полето е задължително.");
                });

            RegisterServiceLayer(services);
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //var serviceProvider = app.ApplicationServices;
            //var townsEnv = serviceProvider.GetService<ITownsService>();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.SeedDatabaseAsync();
        }
        private void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<ICalendarService, CalendarService>();
            //Can return error
            services.AddScoped<ITownsService, TownsService>();

        }
        
    }
}
