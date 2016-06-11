namespace MichaelsPlace.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SoftDelete : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Case_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Article_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_Item_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "DynamicFilter_ToDo_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
                "dbo.Situations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Memento = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Situation_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Person_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AlterTableAnnotations(
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
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Organization_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.Cases", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Situations", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.People", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "IsDeleted");
            DropColumn("dbo.People", "IsDeleted");
            DropColumn("dbo.Situations", "IsDeleted");
            DropColumn("dbo.Items", "IsDeleted");
            DropColumn("dbo.Cases", "IsDeleted");
            AlterTableAnnotations(
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
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Organization_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Person_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
                "dbo.Situations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Memento = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Situation_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Article_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_Item_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "DynamicFilter_ToDo_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AlterTableAnnotations(
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
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Case_SoftDelete",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
