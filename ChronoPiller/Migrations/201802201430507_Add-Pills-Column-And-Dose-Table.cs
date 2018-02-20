namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPillsColumnAndDoseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Doses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicineBoxId = c.Int(nullable: false),
                        Pills = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicineBoxes", t => t.MedicineBoxId, cascadeDelete: true)
                .Index(t => t.MedicineBoxId);
            
            AddColumn("dbo.MedicineBoxes", "PillsInBox", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Doses", "MedicineBoxId", "dbo.MedicineBoxes");
            DropIndex("dbo.Doses", new[] { "MedicineBoxId" });
            DropColumn("dbo.MedicineBoxes", "PillsInBox");
            DropTable("dbo.Doses");
        }
    }
}
