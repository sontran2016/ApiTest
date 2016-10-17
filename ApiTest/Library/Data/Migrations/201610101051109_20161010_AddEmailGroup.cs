namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161010_AddEmailGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        GroupContactId = c.Int(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupContact", t => t.GroupContactId)
                .Index(t => t.GroupContactId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactList", "GroupContactId", "dbo.GroupContact");
            DropIndex("dbo.ContactList", new[] { "GroupContactId" });
            DropTable("dbo.ContactList");
            DropTable("dbo.GroupContact");
        }
    }
}
