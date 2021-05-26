namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameDateColumnsInScheduleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "FromTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "ToTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "FromDate");
            DropColumn("dbo.Schedules", "ToDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "ToDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "FromDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "ToTime");
            DropColumn("dbo.Schedules", "FromTime");
        }
    }
}
