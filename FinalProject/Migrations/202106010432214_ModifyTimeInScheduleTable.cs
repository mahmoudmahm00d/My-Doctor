namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyTimeInScheduleTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "FromTime", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Schedules", "ToTime", c => c.String(nullable: false, maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "ToTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schedules", "FromTime", c => c.DateTime(nullable: false));
        }
    }
}
