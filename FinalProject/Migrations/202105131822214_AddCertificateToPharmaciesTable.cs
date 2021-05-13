namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCertificateToPharmaciesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pharmacies", "Certificate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pharmacies", "Certificate");
        }
    }
}
