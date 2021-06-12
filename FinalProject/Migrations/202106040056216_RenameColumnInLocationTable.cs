namespace FinalProject.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenameColumnInLocationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Longtude", c => c.Double(nullable: false));
            DropColumn("dbo.Locations", "Langtude");
        }

        public override void Down()
        {
            AddColumn("dbo.Locations", "Langtude", c => c.Double(nullable: false));
            DropColumn("dbo.Locations", "Longtude");
        }
    }
}
