using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StrongStart.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StrongStart.Models;

namespace StrongStart
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration[("ConnectionStrings:DefaultConnection")]));
            services.AddDbContext<StrongStartContext>(options =>
                options.UseSqlServer(
                    Configuration[("ConnectionStrings:StrongStartDB")]));
            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSession();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Volunteer", "RPC", "Employee","Trainer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //add admin user account to application
            ApplicationUser userAdmin = await UserManager.FindByEmailAsync("admin@StrongStart.com");

            if (userAdmin == null)
            {
                userAdmin = new ApplicationUser()
                {
                    UserName = "admin@StrongStart.com",
                    Email = "admin@StrongStart.com",
                };
                await UserManager.CreateAsync(userAdmin, "Admin@123");
            }
            await UserManager.AddToRoleAsync(userAdmin, "Admin");


            ApplicationUser userVolunteer = await UserManager.FindByEmailAsync("wangqiao921105@gmail.com");

            if (userVolunteer == null)
            {
                userVolunteer = new ApplicationUser()
                {
                    UserName= "wangqiao921105@gmail.com",
                    Email = "wangqiao921105@gmail.com",
                    FirstName = "Qiao",
                    LastName = "Wang",
                    Address = "5 Rittenhouse Rd",
                    City ="Kitchener",
                    Province = "Ontario",
                };
                await UserManager.CreateAsync(userVolunteer, "Wq19921105!");
            }
            await UserManager.AddToRoleAsync(userVolunteer, "Volunteer");

            ApplicationUser userRPC = await UserManager.FindByEmailAsync("rpc@StrongStart.com");

            if (userRPC == null)
            {
                userRPC = new ApplicationUser()
                {
                    UserName = "rpc@StrongStart.com",
                    Email = "rpc@StrongStart.com",
                };
                await UserManager.CreateAsync(userRPC, "Rpc@123");
            }
            await UserManager.AddToRoleAsync(userRPC, "RPC");

            //add user Employee in application
            ApplicationUser userEmployee = await UserManager.FindByEmailAsync("Employee@StrongStart.com");

            if (userEmployee == null)
            {
                userEmployee = new ApplicationUser()
                {
                    UserName= "Employee@StrongStart.com",
                    Email = "Employee@StrongStart.com",
                };
                await UserManager.CreateAsync(userEmployee, "Employee@123");
            }
            await UserManager.AddToRoleAsync(userEmployee, "Employee");

            //add user Trainer in application
            ApplicationUser userTrainer = await UserManager.FindByEmailAsync("trainer@StrongStart.com");

            if (userTrainer == null)
            {
                userTrainer = new ApplicationUser()
                {
                    UserName = "Trainer@StrongStart.com",
                    Email = "Trainer@StrongStart.com",
                };
                await UserManager.CreateAsync(userTrainer, "Trainer@123");
            }
            await UserManager.AddToRoleAsync(userTrainer, "Trainer");

        }
    }
}
