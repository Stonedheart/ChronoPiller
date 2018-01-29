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
            GlobalConfiguration.Configuration.UseSqlServerStorage("MailerDb", new SqlServerStorageOptions()
            {
                QueuePollInterval = TimeSpan.FromSeconds(1)
            });

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}