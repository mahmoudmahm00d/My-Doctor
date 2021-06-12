namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyAppoinmentTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Appointments", "Date", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments", "AppointmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropPrimaryKey("dbo.Appointments");
            AlterColumn("dbo.Appointments", "Date", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Appointments", "AppointmentId");
            AddForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments", "AppointmentId", cascadeDelete: true);
        }
    }
}
