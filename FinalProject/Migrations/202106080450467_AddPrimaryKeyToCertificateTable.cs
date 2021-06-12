namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrimaryKeyToCertificateTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Certifcates");
            AddColumn("dbo.Certifcates", "CertifcateID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Certifcates", "CertifcateDescription", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Certifcates", "CertifcateID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Certifcates");
            AlterColumn("dbo.Certifcates", "CertifcateDescription", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Certifcates", "CertifcateID");
            AddPrimaryKey("dbo.Certifcates", new[] { "UserId", "CertifcateDescription" });
        }
    }
}
