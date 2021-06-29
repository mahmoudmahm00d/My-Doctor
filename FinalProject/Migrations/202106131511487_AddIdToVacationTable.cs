namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdToVacationTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Vacations");
            AddColumn("dbo.Vacations", "VacationId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Vacations", "VacationId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Vacations");
            DropColumn("dbo.Vacations", "VacationId");
            AddPrimaryKey("dbo.Vacations", new[] { "ClinicId", "FromDate" });
        }
    }
}
