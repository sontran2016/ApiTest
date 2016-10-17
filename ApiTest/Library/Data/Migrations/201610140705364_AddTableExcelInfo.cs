namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableExcelInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExcelInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartnerID = c.String(),
                        Style = c.String(),
                        PartnerSKU = c.String(),
                        UPC = c.String(),
                        Description = c.String(),
                        ColorCode = c.String(),
                        ColorDesc = c.String(),
                        SizeCode = c.String(),
                        SizeDescription = c.String(),
                        SizeClassDescription = c.String(),
                        WeightLBS = c.String(),
                        PreviewImageURL = c.String(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExcelInfo");
        }
    }
}
