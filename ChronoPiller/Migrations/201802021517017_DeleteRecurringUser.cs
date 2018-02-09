namespace ChronoPiller.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRecurringUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prescriptions", "User_Id", "dbo.Users");
            DropIndex("dbo.Prescriptions", new[] { "User_Id" });
            DropColumn("dbo.Prescriptions", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "User_Id", c => c.Int());
            CreateIndex("dbo.Prescriptions", "User_Id");
            AddForeignKey("dbo.Prescriptions", "User_Id", "dbo.Users", "Id");
        }
    }
}
