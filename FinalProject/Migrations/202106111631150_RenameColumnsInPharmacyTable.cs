namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnsInPharmacyTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacies", "Longtude", c => c.Double(nullable: false));
            DropColumn("dbo.Pharmacies", "Langtude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pharmacies", "Langtude", c => c.Double(nullable: false));
            DropColumn("dbo.Pharmacies", "Longtude");
        }
    }
}
