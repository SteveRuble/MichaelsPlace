namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        CreatedBy = c.String(nullable: false),
                        CreatedUtc = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Memento = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.CaseUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Situation_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Case_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .Index(t => t.Situation_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Case_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                "dbo.UserCaseItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Case_Id = c.String(maxLength: 128),
                        Item_Id = c.Int(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Case_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.User_Id);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.Invitee_Id)
                .Index(t => t.Case_Id)
                .Index(t => t.CaseItem_Id)
                .Index(t => t.Case_Id1)
                .Index(t => t.Invitee_Id);
            
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
            DropForeignKey("dbo.Notifications", "Invitee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "Case_Id1", "dbo.Cases");
            DropForeignKey("dbo.Notifications", "CaseItem_Id", "dbo.CaseItems");
            DropForeignKey("dbo.Notifications", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.CaseUsers", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.UserCaseItems", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCaseItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.UserCaseItems", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CaseUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CaseUsers", "Situation_Id", "dbo.Situations");
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
            DropIndex("dbo.UserCaseItems", new[] { "User_Id" });
            DropIndex("dbo.UserCaseItems", new[] { "Item_Id" });
            DropIndex("dbo.UserCaseItems", new[] { "Case_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CaseUsers", new[] { "Case_Id" });
            DropIndex("dbo.CaseUsers", new[] { "User_Id" });
            DropIndex("dbo.CaseUsers", new[] { "Situation_Id" });
            DropIndex("dbo.CaseItems", new[] { "Case_Id" });
            DropIndex("dbo.CaseItems", new[] { "Item_Id" });
            DropTable("dbo.ItemSituations");
            DropTable("dbo.SituationMournerTags");
            DropTable("dbo.SituationLossTags");
            DropTable("dbo.SituationDemographicTags");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Notifications");
            DropTable("dbo.UserCaseItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CaseUsers");
            DropTable("dbo.Tags");
            DropTable("dbo.Situations");
            DropTable("dbo.Items");
            DropTable("dbo.CaseItems");
            DropTable("dbo.Cases");
        }
    }
}
