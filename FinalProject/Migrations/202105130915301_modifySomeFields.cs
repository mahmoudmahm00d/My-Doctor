namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifySomeFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics");
            DropPrimaryKey("dbo.Schedules");
            AddColumn("dbo.Clinics", "Certificate", c => c.String());
            AddColumn("dbo.Schedules", "ScheduleId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Schedules", "ScheduleId");
            AddForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics", "ClinicId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics");
            DropPrimaryKey("dbo.Schedules");
            DropColumn("dbo.Schedules", "ScheduleId");
            DropColumn("dbo.Clinics", "Certificate");
            AddPrimaryKey("dbo.Schedules", "ClinicId");
            AddForeignKey("dbo.Schedules", "ClinicId", "dbo.Clinics", "ClinicId");
        }
    }
}
