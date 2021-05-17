namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBirthToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Birth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Birth");
        }
    }
}
