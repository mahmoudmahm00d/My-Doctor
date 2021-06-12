namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStingLengthInManagerTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Managers", "ManagerPassword", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Managers", "ManagerPassword", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
