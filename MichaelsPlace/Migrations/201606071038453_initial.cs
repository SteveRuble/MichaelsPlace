namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LineOne = c.String(nullable: false, maxLength: 200),
                        LineTwo = c.String(maxLength: 200),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 2),
                        Zip = c.String(nullable: false, maxLength: 12),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        CreatedBy = c.String(nullable: false),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.CaseItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Item_Id = c.Int(),
                        Case_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Situations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Memento = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        GuidanceLabel = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Person_Id = c.String(nullable: false, maxLength: 128),
                        Situation_Id = c.Int(nullable: false),
                        Case_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Situation_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsDisabled = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubscriptionName = c.String(),
                        IsEmailRequested = c.Boolean(),
                        IsSmsRequested = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        FaxNumber = c.String(),
                        Notes = c.String(),
                        Address_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .Index(t => t.Address_Id);
            
            CreateTable(
                "dbo.PersonCaseItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Case_Id = c.String(maxLength: 128),
                        Item_Id = c.Int(),
                        Person_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .Index(t => t.Case_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        ToAddress = c.String(),
                        Subject = c.String(),
                        ToPhoneNumber = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Case_Id = c.String(maxLength: 128),
                        CaseItem_Id = c.Int(),
                        Case_Id1 = c.String(maxLength: 128),
                        Invitee_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .ForeignKey("dbo.CaseItems", t => t.CaseItem_Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id1)
                .ForeignKey("dbo.People", t => t.Invitee_Id)
                .Index(t => t.Case_Id)
                .Index(t => t.CaseItem_Id)
                .Index(t => t.Case_Id1)
                .Index(t => t.Invitee_Id);
            
            CreateTable(
                "dbo.HistoricalEvents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TimestampUtc = c.DateTimeOffset(nullable: false, precision: 7),
                        EventType = c.String(nullable: false),
                        ContentJson = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsDisabled = c.Boolean(nullable: false),
                        Email = c.String(),
                        IsLockedOut = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SituationDemographicTags",
                c => new
                    {
                        Situation_Id = c.Int(nullable: false),
                        DemographicTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Situation_Id, t.DemographicTag_Id })
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.DemographicTag_Id, cascadeDelete: true)
                .Index(t => t.Situation_Id)
                .Index(t => t.DemographicTag_Id);
            
            CreateTable(
                "dbo.SituationLossTags",
                c => new
                    {
                        Situation_Id = c.Int(nullable: false),
                        LossTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Situation_Id, t.LossTag_Id })
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.LossTag_Id, cascadeDelete: true)
                .Index(t => t.Situation_Id)
                .Index(t => t.LossTag_Id);
            
            CreateTable(
                "dbo.SituationMournerTags",
                c => new
                    {
                        Situation_Id = c.Int(nullable: false),
                        MournerTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Situation_Id, t.MournerTag_Id })
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.MournerTag_Id, cascadeDelete: true)
                .Index(t => t.Situation_Id)
                .Index(t => t.MournerTag_Id);
            
            CreateTable(
                "dbo.ItemSituations",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        Situation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Id, t.Situation_Id })
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.Situation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notifications", "Invitee_Id", "dbo.People");
            DropForeignKey("dbo.Notifications", "Case_Id1", "dbo.Cases");
            DropForeignKey("dbo.Notifications", "CaseItem_Id", "dbo.CaseItems");
            DropForeignKey("dbo.Notifications", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.PersonCases", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.PersonCases", "Situation_Id", "dbo.Situations");
            DropForeignKey("dbo.PersonCases", "Person_Id", "dbo.People");
            DropForeignKey("dbo.PersonCaseItems", "Person_Id", "dbo.People");
            DropForeignKey("dbo.PersonCaseItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.PersonCaseItems", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.Situations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.People", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Cases", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.People");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserPreferences", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CaseItems", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.CaseItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ItemSituations", "Situation_Id", "dbo.Situations");
            DropForeignKey("dbo.ItemSituations", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.SituationMournerTags", "MournerTag_Id", "dbo.Tags");
            DropForeignKey("dbo.SituationMournerTags", "Situation_Id", "dbo.Situations");
            DropForeignKey("dbo.SituationLossTags", "LossTag_Id", "dbo.Tags");
            DropForeignKey("dbo.SituationLossTags", "Situation_Id", "dbo.Situations");
            DropForeignKey("dbo.SituationDemographicTags", "DemographicTag_Id", "dbo.Tags");
            DropForeignKey("dbo.SituationDemographicTags", "Situation_Id", "dbo.Situations");
            DropIndex("dbo.ItemSituations", new[] { "Situation_Id" });
            DropIndex("dbo.ItemSituations", new[] { "Item_Id" });
            DropIndex("dbo.SituationMournerTags", new[] { "MournerTag_Id" });
            DropIndex("dbo.SituationMournerTags", new[] { "Situation_Id" });
            DropIndex("dbo.SituationLossTags", new[] { "LossTag_Id" });
            DropIndex("dbo.SituationLossTags", new[] { "Situation_Id" });
            DropIndex("dbo.SituationDemographicTags", new[] { "DemographicTag_Id" });
            DropIndex("dbo.SituationDemographicTags", new[] { "Situation_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Notifications", new[] { "Invitee_Id" });
            DropIndex("dbo.Notifications", new[] { "Case_Id1" });
            DropIndex("dbo.Notifications", new[] { "CaseItem_Id" });
            DropIndex("dbo.Notifications", new[] { "Case_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Person_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Item_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Case_Id" });
            DropIndex("dbo.Organizations", new[] { "Address_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.UserPreferences", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Id" });
            DropIndex("dbo.People", new[] { "Organization_Id" });
            DropIndex("dbo.PersonCases", new[] { "Case_Id" });
            DropIndex("dbo.PersonCases", new[] { "Situation_Id" });
            DropIndex("dbo.PersonCases", new[] { "Person_Id" });
            DropIndex("dbo.Situations", new[] { "Organization_Id" });
            DropIndex("dbo.CaseItems", new[] { "Case_Id" });
            DropIndex("dbo.CaseItems", new[] { "Item_Id" });
            DropIndex("dbo.Cases", new[] { "Organization_Id" });
            DropTable("dbo.ItemSituations");
            DropTable("dbo.SituationMournerTags");
            DropTable("dbo.SituationLossTags");
            DropTable("dbo.SituationDemographicTags");
            DropTable("dbo.UserModels");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HistoricalEvents");
            DropTable("dbo.Notifications");
            DropTable("dbo.PersonCaseItems");
            DropTable("dbo.Organizations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.People");
            DropTable("dbo.PersonCases");
            DropTable("dbo.Tags");
            DropTable("dbo.Situations");
            DropTable("dbo.Items");
            DropTable("dbo.CaseItems");
            DropTable("dbo.Cases");
            DropTable("dbo.Addresses");
        }
    }
}
