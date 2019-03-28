using System;
using IdentityDemo.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IdentityDemo.Areas.Identity.Services;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

using IdentityDemo.Areas.Identity.Data;

[assembly: HostingStartup(typeof(IdentityDemo.Areas.Identity.IdentityHostingStartup))]
namespace IdentityDemo.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDbContextPool<IdentityDemoIdentityDbContext>( // replace "YourDbContext" with the class name of your DbContext
                    options => options.UseMySql(context.Configuration.GetConnectionString("DefaultConnection"), // replace with your Connection String
                        mySqlOptions =>
                        {
                            mySqlOptions.ServerVersion(new Version(5, 7, 25), ServerType.MySql) // replace with your Server Version and Type
                                        .DisableBackslashEscaping();
                        }
                ));                

                services.AddDefaultIdentity<OneRideUser>(config => {
                            config.SignIn.RequireConfirmedEmail = true;
                        })
                        .AddDefaultUI(UIFramework.Bootstrap4)
                        .AddEntityFrameworkStores<IdentityDemoIdentityDbContext>();

                // from here: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=netcore-cli
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
                });                

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });

                // requires
                // using Microsoft.AspNetCore.Identity.UI.Services;
                // using WebPWrecover.Services;
                services.AddTransient<IEmailSender, EmailSender>();

            });
        }
    }
}