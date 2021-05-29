namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityToMedicineTypesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Medicines", "MedicineTypeId", "dbo.MedicineTypes");
            DropPrimaryKey("dbo.MedicineTypes");
            AlterColumn("dbo.MedicineTypes", "MedicineTypeId", c => c.Byte(nullable: false, identity: true));
            AddPrimaryKey("dbo.MedicineTypes", "MedicineTypeId");
            AddForeignKey("dbo.Medicines", "MedicineTypeId", "dbo.MedicineTypes", "MedicineTypeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Medicines", "MedicineTypeId", "dbo.MedicineTypes");
            DropPrimaryKey("dbo.MedicineTypes");
            AlterColumn("dbo.MedicineTypes", "MedicineTypeId", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.MedicineTypes", "MedicineTypeId");
            AddForeignKey("dbo.Medicines", "MedicineTypeId", "dbo.MedicineTypes", "MedicineTypeId", cascadeDelete: true);
        }
    }
}
