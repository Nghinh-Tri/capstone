using ESMS.Application.Services.Positions;
using ESMS.Application.Services.Projects;
using ESMS.Application.System.Certifications;
using ESMS.Application.System.Employees;
using ESMS.Application.System.Skills;
using ESMS.Data.EF;
using ESMS.Data.Entities;
using ESMS.Utilities.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESMS.BackendAPI
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
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddDbContext<ESMSDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

            //Use Identity
            services.AddIdentity<Employee, Role>()
                .AddEntityFrameworkStores<ESMSDbContext>()
                .AddDefaultTokenProviders();

            //Declare DI
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IPositionService, PositionService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ICertificationService, CertificationService>();
            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<UserManager<Employee>, UserManager<Employee>>();
            services.AddTransient<SignInManager<Employee>, SignInManager<Employee>>();
            services.AddTransient<RoleManager<Role>, RoleManager<Role>>();

            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Capstone Project", Version = "v1" });
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger CapstoneProject V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapGet("/echo",
                context => context.Response.WriteAsync("echo"))
                .RequireCors("MyPolicy");

                endpoints.MapControllers()
                         .RequireCors("MyPolicy");
            });
        }
    }
}