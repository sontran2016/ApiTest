namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSosLogTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RideInformation", "SafetySettingId", "dbo.SafetySetting");
            DropForeignKey("dbo.SosGeolocation", "RideInformationId", "dbo.RideInformation");
            DropIndex("dbo.RideInformation", new[] { "SafetySettingId" });
            DropIndex("dbo.SosGeolocation", new[] { "RideInformationId" });
            CreateTable(
                "dbo.LogSos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YayYoId = c.Int(nullable: false),
                        SafetySettingId = c.Int(),
                        UpdatedOnUtc = c.DateTime(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SafetySetting", t => t.SafetySettingId)
                .Index(t => t.SafetySettingId);
            
            CreateTable(
                "dbo.LogRideInformation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YayYoId = c.Int(nullable: false),
                        DriverName = c.String(),
                        CarMake = c.String(),
                        CarModel = c.String(),
                        CarColor = c.String(),
                        CarLicense = c.String(),
                        LocationPickup = c.String(maxLength: 100),
                        LocationDestination = c.String(maxLength: 100),
                        TimePickup = c.DateTime(),
                        TimeEta = c.Int(nullable: false),
                        LogSosId = c.Int(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LogSos", t => t.LogSosId)
                .Index(t => t.LogSosId);
            
            CreateTable(
                "dbo.LogSosGeolocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(maxLength: 100),
                        LogSosId = c.Int(),
                        CreatedOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LogSos", t => t.LogSosId)
                .Index(t => t.LogSosId);
            
            AddColumn("dbo.YayYoApplication", "CreatedOnUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.SafetySetting", "CreatedOnUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contact", "CreatedOnUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.ScheduleTask", "CreatedOnUtc", c => c.DateTime(nullable: false));
            DropColumn("dbo.YayYoApplication", "CreatedDateUtc");
            DropColumn("dbo.SafetySetting", "CreatedDateUtc");
            DropColumn("dbo.Contact", "CreatedDateUtc");
            DropColumn("dbo.ScheduleTask", "CreatedDateUtc");
            DropTable("dbo.RideInformation");
            DropTable("dbo.SosGeolocation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SosGeolocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GeoLocation = c.String(maxLength: 100),
                        RideInformationId = c.Int(),
                        CreatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ScheduleTask", "CreatedDateUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contact", "CreatedDateUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.SafetySetting", "CreatedDateUtc", c => c.DateTime(nullable: false));
            AddColumn("dbo.YayYoApplication", "CreatedDateUtc", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.LogSosGeolocation", "LogSosId", "dbo.LogSos");
            DropForeignKey("dbo.LogSos", "SafetySettingId", "dbo.SafetySetting");
            DropForeignKey("dbo.LogRideInformation", "LogSosId", "dbo.LogSos");
            DropIndex("dbo.LogSosGeolocation", new[] { "LogSosId" });
            DropIndex("dbo.LogRideInformation", new[] { "LogSosId" });
            DropIndex("dbo.LogSos", new[] { "SafetySettingId" });
            DropColumn("dbo.ScheduleTask", "CreatedOnUtc");
            DropColumn("dbo.Contact", "CreatedOnUtc");
            DropColumn("dbo.SafetySetting", "CreatedOnUtc");
            DropColumn("dbo.YayYoApplication", "CreatedOnUtc");
            DropTable("dbo.LogSosGeolocation");
            DropTable("dbo.LogRideInformation");
            DropTable("dbo.LogSos");
            CreateIndex("dbo.SosGeolocation", "RideInformationId");
            CreateIndex("dbo.RideInformation", "SafetySettingId");
            AddForeignKey("dbo.SosGeolocation", "RideInformationId", "dbo.RideInformation", "Id");
            AddForeignKey("dbo.RideInformation", "SafetySettingId", "dbo.SafetySetting", "Id");
        }
    }
}
