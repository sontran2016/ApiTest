namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRideInfoAndLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RideInformation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RideBookId = c.Int(nullable: false),
                        PickupGeoLocation = c.String(maxLength: 100),
                        DestinationGeoLocation = c.String(maxLength: 100),
                        RideBookOnUtc = c.DateTime(),
                        SafetySettingId = c.Int(),
                        CreatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SafetySetting", t => t.SafetySettingId)
                .Index(t => t.SafetySettingId);
            
            CreateTable(
                "dbo.SosGeolocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GeoLocation = c.String(maxLength: 100),
                        RideInformationId = c.Int(),
                        CreatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RideInformation", t => t.RideInformationId)
                .Index(t => t.RideInformationId);
            
            CreateTable(
                "dbo.ScheduleTask",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Seconds = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        LastStartOnUtc = c.DateTime(),
                        LastEndOnUtc = c.DateTime(),
                        LastSuccessOnUtc = c.DateTime(),
                        IsRunning = c.Boolean(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SafetySetting", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.SafetySetting", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.SafetySetting", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SosGeolocation", "RideInformationId", "dbo.RideInformation");
            DropForeignKey("dbo.RideInformation", "SafetySettingId", "dbo.SafetySetting");
            DropIndex("dbo.SosGeolocation", new[] { "RideInformationId" });
            DropIndex("dbo.RideInformation", new[] { "SafetySettingId" });
            DropColumn("dbo.SafetySetting", "PhoneNumber");
            DropColumn("dbo.SafetySetting", "LastName");
            DropColumn("dbo.SafetySetting", "FirstName");
            DropTable("dbo.ScheduleTask");
            DropTable("dbo.SosGeolocation");
            DropTable("dbo.RideInformation");
        }
    }
}
