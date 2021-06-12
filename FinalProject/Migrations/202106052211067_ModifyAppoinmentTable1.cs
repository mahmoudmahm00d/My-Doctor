

namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAppoinmentTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "Symptoms", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Symptoms", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
