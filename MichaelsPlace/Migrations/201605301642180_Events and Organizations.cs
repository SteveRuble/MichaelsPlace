namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventsandOrganizations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhaneNumber = c.String(),
                        FaxNumber = c.String(),
                        Notes = c.String(),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineOne = c.String(nullable: false, maxLength: 200),
                        LineTwo = c.String(nullable: false, maxLength: 200),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 2),
                        Zip = c.String(nullable: false, maxLength: 12),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventType = c.Int(),
                        IsEmailRequested = c.Boolean(),
                        IsSmsRequested = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TimestampUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ContentJson = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Cases", "Organization_Id", c => c.Int());
            AddColumn("dbo.Situations", "Name", c => c.String());
            AddColumn("dbo.Situations", "Organization_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsDisabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Organization_Id", c => c.Int());
            CreateIndex("dbo.Cases", "Organization_Id");
            CreateIndex("dbo.Situations", "Organization_Id");
            CreateIndex("dbo.AspNetUsers", "Organization_Id");
            AddForeignKey("dbo.Cases", "Organization_Id", "dbo.Organizations", "Id");
            AddForeignKey("dbo.Situations", "Organization_Id", "dbo.Organizations", "Id");
            AddForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPreferences", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Situations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Cases", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.UserPreferences", new[] { "User_Id" });
            DropIndex("dbo.Organizations", new[] { "Address_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Organization_Id" });
            DropIndex("dbo.Situations", new[] { "Organization_Id" });
            DropIndex("dbo.Cases", new[] { "Organization_Id" });
            DropColumn("dbo.AspNetUsers", "Organization_Id");
            DropColumn("dbo.AspNetUsers", "IsDisabled");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.Situations", "Organization_Id");
            DropColumn("dbo.Situations", "Name");
            DropColumn("dbo.Cases", "Organization_Id");
            DropTable("dbo.Events");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.Addresses");
            DropTable("dbo.Organizations");
        }
    }
}
