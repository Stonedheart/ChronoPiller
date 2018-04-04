namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationRelatedTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Doses", "MedicineBoxId", "dbo.MedicineBoxes");
            DropIndex("dbo.Doses", new[] { "MedicineBoxId" });
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PrescriptionId = c.Int(nullable: false),
                        NotificationTypeId = c.Int(nullable: false),
                        HangFireId = c.String(),
                        Cron = c.String(),
                        NextExecution = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PrescriptionId)
                .Index(t => t.NotificationTypeId);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Doses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Doses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicineBoxId = c.Int(nullable: false),
                        Pills = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Notifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Notifications", "NotificationTypeId", "dbo.NotificationTypes");
            DropIndex("dbo.Notifications", new[] { "NotificationTypeId" });
            DropIndex("dbo.Notifications", new[] { "PrescriptionId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.Notifications");
            CreateIndex("dbo.Doses", "MedicineBoxId");
            AddForeignKey("dbo.Doses", "MedicineBoxId", "dbo.MedicineBoxes", "Id", cascadeDelete: true);
        }
    }
}
