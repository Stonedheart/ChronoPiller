using System;
using Hangfire;
using Hangfire.SqlServer;
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

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}