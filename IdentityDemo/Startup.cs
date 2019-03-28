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
using IdentityDemo.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IdentityDemo.Areas.Identity.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;



namespace IdentityDemo
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

            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlite(
            //         Configuration.GetConnectionString("DefaultConnection")));

            // other service configurations go here
            // services.AddDbContextPool<ApplicationDbContext>( // replace "YourDbContext" with the class name of your DbContext
            //     options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), // replace with your Connection String
            //         mySqlOptions =>
            //         {
            //             mySqlOptions.ServerVersion(new Version(5, 7, 25), ServerType.MySql) // replace with your Server Version and Type
            //                         .DisableBackslashEscaping();
            //         }
            // ));            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
