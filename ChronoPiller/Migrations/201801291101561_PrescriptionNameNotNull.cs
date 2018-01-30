namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrescriptionNameNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prescriptions", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prescriptions", "Name", c => c.String());
        }
    }
}
