namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRedundantColumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "UserEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserEmail", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
        }
    }
}
