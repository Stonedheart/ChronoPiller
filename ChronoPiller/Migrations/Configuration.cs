using ChronoPiller.Models;

namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ChronoPiller.DAL.ChronoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ChronoPiller.DAL.ChronoDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            using (context)
            {
                context.NotificationTypes.Add(new NotificationType() { Name = "Dose" });
                context.NotificationTypes.Add(new NotificationType() { Name = "OutOfPills" });
                context.NotificationTypes.Add(new NotificationType() { Name = "BeforeMeal" });
                context.NotificationTypes.Add(new NotificationType() { Name = "AfterMeal" });
                context.NotificationTypes.Add(new NotificationType() { Name = "PrescriptionExpiration" });
                context.SaveChanges();
            }
        }
    }
}
