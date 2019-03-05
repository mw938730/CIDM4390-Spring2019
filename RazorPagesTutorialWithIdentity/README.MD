# Steps for __RazorPagesTutorialWithIdentity__

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