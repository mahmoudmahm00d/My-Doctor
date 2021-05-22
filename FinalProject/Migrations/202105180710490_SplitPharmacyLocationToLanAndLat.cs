namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitPharmacyLocationToLanAndLat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacies", "Langtude", c => c.Double(nullable: false));
            AddColumn("dbo.Pharmacies", "Latitude", c => c.Double(nullable: false));
            DropColumn("dbo.Pharmacies", "PharmacyLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pharmacies", "PharmacyLocation", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Pharmacies", "Latitude");
            DropColumn("dbo.Pharmacies", "Langtude");
        }
    }
}
