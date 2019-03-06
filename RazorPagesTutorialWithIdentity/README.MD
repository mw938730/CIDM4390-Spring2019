# Steps for __RazorPagesTutorialWithIdentity__

**Objective**: To create a ASP.NET Core 2.2 Razor Pages app with Identity and publish it to Azure.  Further, switch between Development and Production configurations.

*Steps*:
1. Create a Razor Pages app configured with Identity
2. How to "steal" other web pages and re-create them in Razor Pages
3. Publish your ASP.NET Core app as an App Service on Azure

## Step One: [Create webapp with Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=netcore-cli)

1. Create a directory for your app: `mkdir MyApp`
2. Move into that directory: `cd MyApp`
3. At the command prompe, type this: `dotnet new webapp --auth Individual`
4. Install [SqLite Db Browser](https://sqlitebrowser.org/dl/) to view/work with [SQLite](https://sqlite.org/index.html) database files.
5. Add configuration to the `startup.cs` file by including these lines in `public void ConfigureServices(IServiceCollection services)`

```C#
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
```

6. [Scaffold identity into a Razor project with authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-2.2&tabs=netcore-cli#scaffold-identity-into-a-razor-project-with-authorization):
    * Install the ASP.NET Core scaffolder: `dotnet tool install -g dotnet-aspnet-codegenerator`
    * Add a package reference to [Microsoft.VisualStudio.Web.CodeGeneration.Design](https://www.nuget.org/packages/Microsoft.VisualStudio.Web.CodeGeneration.Design/) your project:
      ```
        dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
        dotnet restore
      ```
    * List the scaffolder options to see what you can *automagically* create in the future: `dotnet aspnet-codegenerator identity -h`
    * Scaffold the Register, Login, and LogOut pages where you replace *[Your-App-Name-Here]* with your namespace/appname:
      ```
        dotnet aspnet-codegenerator identity -dc [Your-App-Name-Here].Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout"
      ```
      for instance: `dotnet aspnet-codegenerator identity -dc RazorPagesTutorialWithIdentity.Data.ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout"`
    * You'll need to provide links to these pages now.  However, for now, run the app: `dotnet run`

7. Examine the `/Areas/Identity/Pages/Account/Register.cshtml`, `/Areas/Identity/Pages/Account/Login.cshtml`, and `/Areas/Identity/Pages/Account/Logout.cshtml` Pages
8. Let's test it out:
    * Edit the `/Pages/Privacy.cshtml.cs` code-behind file to place the `[Authorize]` decorator over the class declaration:
      ```C#
        using Microsoft.AspNetCore.Authorization;
        using Microsoft.AspNetCore.Mvc.RazorPages;

        namespace WebApp1.Pages
        {
            [Authorize]
            public class PrivacyModel : PageModel
            {
                public void OnGet()
                {
                }
            }
        }
      ```
    * Check the behavior: If you are signed in, sign out. Run the app and select the Privacy link. You are redirected to the login page.

## Step Two: [Create an ASP.NET Core app in App Service](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-dotnetcore)

1. Azure testing
2. make sure you place `app.UseDeveloperExceptionPage();` outside of the if statement for development mode in the config in startup
3. Also, follow the advice here: [Azure Web Sites Git Deploy For Multiple Project Solutions](https://www.devtrends.co.uk/blog/azure-web-sites-git-deploy-for-multiple-project-solutions). See here too [Kudu Customizing Deployments](https://github.com/projectkudu/kudu/wiki/Customizing-deployments)