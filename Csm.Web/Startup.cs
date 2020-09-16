using AutoMapper;
using Csm.Domain.Config;
using Csm.Domain.Services;
using Csm.Domain.SynchronizeApi.Service;
using Csm.Dto.Entities;
using Csm.Web.Data;
using Csm.Web.Extensions;
using CSM.Dal.Helper;
using CSM.Dal.Internal;
using CSM.Dal.Repositories;
using CSM.Dal.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
            //Version of PgSql requried for the Identity to work.
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Csmdb"),
            options => options.SetPostgresVersion(new Version(9, 6))));

            //Identity Api password defination
            services.AddIdentity<ApplicationUserDomain, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 2;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            //for identity service httpcontext in domain library.
            services.AddHttpContextAccessor();

            //DI services and dal
            services.AddTransient<IDashboard, DashboardSites>();
            services.AddTransient<IDashboard, DashBoardUser>();
            services.AddTransient<IReportQuery, ReportQuery>();
            services.AddTransient<IReportServices, ReportServices>();
            services.AddTransient<IMonitoringRepository, MonitoringRepository>();
            services.AddTransient<IDataAccess, DataAccess>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
            services.AddTransient<ISyncronizeService, SyncronizeService>();
            services.AddTransient<ICreateFile, CreateFile>();
            services.AddTransient<IDataInsertion, DataInsertion>();
            services.AddTransient<IImageExtractor, ImageExtractor>();
            services.AddTransient<IReadSqliteData, ReadSqliteData>();
            services.AddTransient<ISynchronizer, Synchronizer>();
            services.AddTransient<IReadSqlite, ReadSqlite>();
            services.AddScoped<ISqlitePath, SqlitePath>();


            //AutoMapper not used now
            //services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            //CSRF Global Filter.
            services.AddMvc(options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            // configure strongly typed settings objects
            services.Configure<CsmSettings>(options => Configuration.GetSection("CsmSettings").Bind(options)); //for DI in TokenGenerator.
            var csmSettings = Configuration.GetSection("CsmSettings").Get<CsmSettings>(); //for Jwt Auth Middleware.

            // connection string 
            services.Configure<CsmData>(options => Configuration.GetSection("ConnectionStrings").Bind(options));

            //jtw Injection
            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = csmSettings.Issuer,
                        ValidAudience = csmSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(csmSettings.SecretKey)),
                        ValidateLifetime = true
                    };
                }
                );

            //Swagger
           // services.AddSwaggerGen(setup =>
           //{
           //    setup.SwaggerDoc(
           //            "v1",
           //            new OpenApiInfo
           //            {
           //                Title = "Construction Site Monitoring API",
           //                Version = "v1"
           //            });
           //    setup.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
           //    {
           //        Type = SecuritySchemeType.Http,
           //        BearerFormat = "JWT",
           //        In = ParameterLocation.Header,
           //        Scheme = "bearer"
           //    });
           //    setup.OperationFilter<AuthenticationRequirementsOperationFilter>();
           //});
        }

        // Middleware: This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error"); // In Production when error occured display error page From Error Controller.
                app.UseStatusCodePagesWithReExecute("/Error/{0}"); //For undefined Url like foo/bar?email=abc@mail.com.

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
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
        }
    }
}