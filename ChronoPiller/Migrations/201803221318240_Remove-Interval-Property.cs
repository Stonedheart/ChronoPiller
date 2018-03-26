namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIntervalProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PrescriptedMedicines", "Interval");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrescriptedMedicines", "Interval", c => c.Int(nullable: false));
        }
    }
}
