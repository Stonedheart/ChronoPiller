using System;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace ChronoPiller
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChronoPiller.DAL.ChronoPillerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                new SqlServerStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                });
            ConfigureAuth(app);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new ChronoPillerDb());
            app.CreatePerOwinContext<ChronoUserManager>(ChronoUserManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}