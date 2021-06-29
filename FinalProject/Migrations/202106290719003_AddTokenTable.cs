namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TokenProperties",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 255),
                        ExpireDate = c.DateTime(nullable: false),
                        ObjectId = c.Int(),
                        ObjectType = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.Token });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TokenProperties");
        }
    }
}
