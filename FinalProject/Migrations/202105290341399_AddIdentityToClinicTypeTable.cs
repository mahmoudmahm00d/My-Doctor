namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityToClinicTypeTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes");
            DropPrimaryKey("dbo.ClinicTypes");
            AlterColumn("dbo.ClinicTypes", "ClinicTypeId", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.ClinicTypes", "ClinicTypeId");
            AddForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes", "ClinicTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes");
            DropPrimaryKey("dbo.ClinicTypes");
            AlterColumn("dbo.ClinicTypes", "ClinicTypeId", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.ClinicTypes", "ClinicTypeId");
            AddForeignKey("dbo.Clinics", "ClinicTypeId", "dbo.ClinicTypes", "ClinicTypeId", cascadeDelete: true);
        }
    }
}
