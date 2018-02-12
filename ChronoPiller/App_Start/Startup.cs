using System;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChronoPiller.Startup))]
namespace ChronoPiller
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage(
    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChronoPiller.DAL.ChronoPillerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
    new SqlServerStorageOptions
    {
        QueuePollInterval = TimeSpan.FromSeconds(1)
    });
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ChronoDbContext.Create);
            app.CreatePerOwinContext<ChronoUserManager>(ChronoUserManager.Create);
            app.CreatePerOwinContext<ChronoSignInManager>(ChronoSignInManager.Create);
            app.CreatePerOwinContext<RoleManager<ChronoRole, int>>(
                (options, context) =>
                    new RoleManager<ChronoRole, int>(new RoleStore<ChronoRole, int, ChronoUserRole>(context.Get<ChronoDbContext>())));
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ChronoUserManager, ChronoUser, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => (id.GetUserId<int>()))
                }
            });
        }
    }
}