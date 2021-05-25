namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityToAlmostAllTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes");
            DropPrimaryKey("dbo.Appointments");
            DropPrimaryKey("dbo.ClinicTypes");
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Int(nullable: false,identity:true));
            AlterColumn("dbo.ClinicTypes", "ClinicTypeId", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddPrimaryKey("dbo.ClinicTypes", "ClinicTypeId");
            AddForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments", "AppointmentId", cascadeDelete: true);
            AddForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes", "ClinicTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes");
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.ClinicTypes");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.ClinicTypes", "ClinicTypeId", c => c.Byte(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClinicTypes", "ClinicTypeId");
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes", "ClinicTypeId", cascadeDelete: true);
            AddForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments", "AppointmentId", cascadeDelete: true);
        }
    }
}
