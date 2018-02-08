using System;
using System.Security.Claims;
using System.Web.Helpers;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;

namespace ChronoPiller
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ChronoPiller.DAL.ChronoPillerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                new SqlServerStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                });

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            var options = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("Account/Login")
            };
            app.UseCookieAuthentication(options);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Hardcoded for now 

//            app.UseGoogleAuthentication(
//                clientId: "sijfnoejnio",
//                clientSecret: "jmpfoaejfaef");

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}