using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Csm.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Csm.Web.Models;
using AutoMapper;
using DataAccessLibrary.DataAccessLayer.DataAccess;
using Csm.Services.ServiceInterface;
using Csm.Services.ServicesAccess;

namespace Csm.Web
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
            services.AddDbContextPool<ApplicationDbContext>(o => o.UseNpgsql(Configuration.GetConnectionString("Csmdb"),
            options => options.SetPostgresVersion(new Version(9, 6))));

            //Identity Api password defination
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //personam DI
            services.AddTransient<IInventory, Inventory>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // services.AddSwaggerGen(setup =>
            //{
            //    setup.SwaggerDoc(
            //            "v1",
            //            new OpenApiInfo
            //            {
            //                Title = "Cross Site Monitoring API",
            //                Version = "v1"
            //            });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            { 
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSwagger();
            //app.UseSwaggerUI(x =>
            //{
            //    x.SwaggerEndpoint("/swagger/v1/swagger.json", "CSM API v1");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //try
            //{
            //    System.IO.File.WriteAllText("browsersync-update.txt", DateTime.Now.ToString());
            //}
            //catch
            //{

            //}
        }
    }
}

//services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        Configuration.GetConnectionString("DefaultConnection")));