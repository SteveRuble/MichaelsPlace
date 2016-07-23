namespace MichaelsPlace.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeMigrations : DbMigration
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
                        IsDeleted = c.Boolean(nullable: false),
                        Organization_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Case_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
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
                        IsDeleted = c.Boolean(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Article_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Item_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ToDo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DisplayName = c.String(),
                        Description = c.String(),
                        ContextId = c.Int(),
                        ContextId1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tags", t => t.ContextId)
                .ForeignKey("dbo.Tags", t => t.ContextId1)
                .Index(t => t.ContextId)
                .Index(t => t.ContextId1);
            
            CreateTable(
                "dbo.PersonCases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Person_Id = c.String(nullable: false, maxLength: 128),
                        Relationship_Id = c.Int(nullable: false),
                        Case_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.Tags", t => t.Relationship_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cases", t => t.Case_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Relationship_Id)
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
                        IsDeleted = c.Boolean(nullable: false),
                        Organization_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Person_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
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
                        IsDeleted = c.Boolean(nullable: false),
                        Address_Id = c.Int(),
                        Context_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Organization_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.Tags", t => t.Context_Id)
                .Index(t => t.Address_Id)
                .Index(t => t.Context_Id);
            
            CreateTable(
                "dbo.OrganizationPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Person_Id = c.String(maxLength: 128),
                        Relationship_Id = c.Int(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Person_Id)
                .ForeignKey("dbo.Tags", t => t.Relationship_Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Person_Id)
                .Index(t => t.Relationship_Id)
                .Index(t => t.Organization_Id);
            
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
                "dbo.ItemContextTags",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        ContextTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Id, t.ContextTag_Id })
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.ContextTag_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.ContextTag_Id);
            
            CreateTable(
                "dbo.ItemLossTags",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        LossTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Id, t.LossTag_Id })
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.LossTag_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.LossTag_Id);
            
            CreateTable(
                "dbo.ItemRelationshipTags",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        RelationshipTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Id, t.RelationshipTag_Id })
                .ForeignKey("dbo.Items", t => t.Item_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.RelationshipTag_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.RelationshipTag_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Notifications", "Invitee_Id", "dbo.People");
            DropForeignKey("dbo.Notifications", "Case_Id1", "dbo.Cases");
            DropForeignKey("dbo.Notifications", "CaseItem_Id", "dbo.CaseItems");
            DropForeignKey("dbo.Notifications", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.PersonCases", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.PersonCases", "Relationship_Id", "dbo.Tags");
            DropForeignKey("dbo.PersonCases", "Person_Id", "dbo.People");
            DropForeignKey("dbo.PersonCaseItems", "Person_Id", "dbo.People");
            DropForeignKey("dbo.PersonCaseItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.PersonCaseItems", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.People", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationPersons", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.OrganizationPersons", "Relationship_Id", "dbo.Tags");
            DropForeignKey("dbo.OrganizationPersons", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Organizations", "Context_Id", "dbo.Tags");
            DropForeignKey("dbo.Cases", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.AspNetUsers", "Id", "dbo.People");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserPreferences", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CaseItems", "Case_Id", "dbo.Cases");
            DropForeignKey("dbo.CaseItems", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ItemRelationshipTags", "RelationshipTag_Id", "dbo.Tags");
            DropForeignKey("dbo.ItemRelationshipTags", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ItemLossTags", "LossTag_Id", "dbo.Tags");
            DropForeignKey("dbo.ItemLossTags", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.ItemContextTags", "ContextTag_Id", "dbo.Tags");
            DropForeignKey("dbo.ItemContextTags", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.Tags", "ContextId1", "dbo.Tags");
            DropForeignKey("dbo.Tags", "ContextId", "dbo.Tags");
            DropIndex("dbo.ItemRelationshipTags", new[] { "RelationshipTag_Id" });
            DropIndex("dbo.ItemRelationshipTags", new[] { "Item_Id" });
            DropIndex("dbo.ItemLossTags", new[] { "LossTag_Id" });
            DropIndex("dbo.ItemLossTags", new[] { "Item_Id" });
            DropIndex("dbo.ItemContextTags", new[] { "ContextTag_Id" });
            DropIndex("dbo.ItemContextTags", new[] { "Item_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Notifications", new[] { "Invitee_Id" });
            DropIndex("dbo.Notifications", new[] { "Case_Id1" });
            DropIndex("dbo.Notifications", new[] { "CaseItem_Id" });
            DropIndex("dbo.Notifications", new[] { "Case_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Person_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Item_Id" });
            DropIndex("dbo.PersonCaseItems", new[] { "Case_Id" });
            DropIndex("dbo.OrganizationPersons", new[] { "Organization_Id" });
            DropIndex("dbo.OrganizationPersons", new[] { "Relationship_Id" });
            DropIndex("dbo.OrganizationPersons", new[] { "Person_Id" });
            DropIndex("dbo.Organizations", new[] { "Context_Id" });
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
            DropIndex("dbo.PersonCases", new[] { "Relationship_Id" });
            DropIndex("dbo.PersonCases", new[] { "Person_Id" });
            DropIndex("dbo.Tags", new[] { "ContextId1" });
            DropIndex("dbo.Tags", new[] { "ContextId" });
            DropIndex("dbo.CaseItems", new[] { "Case_Id" });
            DropIndex("dbo.CaseItems", new[] { "Item_Id" });
            DropIndex("dbo.Cases", new[] { "Organization_Id" });
            DropTable("dbo.ItemRelationshipTags");
            DropTable("dbo.ItemLossTags");
            DropTable("dbo.ItemContextTags");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HistoricalEvents");
            DropTable("dbo.Notifications");
            DropTable("dbo.PersonCaseItems");
            DropTable("dbo.OrganizationPersons");
            DropTable("dbo.Organizations",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Organization_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.People",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Person_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PersonCases");
            DropTable("dbo.Tags");
            DropTable("dbo.Items",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Article_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_Item_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_ToDo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CaseItems");
            DropTable("dbo.Cases",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Case_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Addresses");
        }
    }
}
