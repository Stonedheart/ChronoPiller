namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrescriptionExpirationDateRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Prescriptions", "ExpirationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "ExpirationDate", c => c.DateTime(nullable: false));
        }
    }
}
