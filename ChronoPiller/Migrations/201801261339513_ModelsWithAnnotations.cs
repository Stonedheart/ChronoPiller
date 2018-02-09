namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsWithAnnotations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicineBoxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicineId = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        ActiveSubstanceAmountInMg = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .Index(t => t.MedicineId);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrescriptedMedicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartUsageDate = c.DateTime(nullable: false),
                        PrescriptedBoxCount = c.Int(nullable: false),
                        Dose = c.Int(nullable: false),
                        Interval = c.Int(nullable: false),
                        PrescriptionId = c.Int(nullable: false),
                        MedicineBoxId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicineBoxes", t => t.MedicineBoxId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId)
                .Index(t => t.MedicineBoxId);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateOfIssue = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrescriptedMedicines", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.PrescriptedMedicines", "MedicineBoxId", "dbo.MedicineBoxes");
            DropForeignKey("dbo.MedicineBoxes", "MedicineId", "dbo.Medicines");
            DropIndex("dbo.Prescriptions", new[] { "User_Id" });
            DropIndex("dbo.PrescriptedMedicines", new[] { "MedicineBoxId" });
            DropIndex("dbo.PrescriptedMedicines", new[] { "PrescriptionId" });
            DropIndex("dbo.MedicineBoxes", new[] { "MedicineId" });
            DropTable("dbo.Users");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.PrescriptedMedicines");
            DropTable("dbo.Medicines");
            DropTable("dbo.MedicineBoxes");
        }
    }
}
