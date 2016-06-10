namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationHasCases : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Situations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.Situations", new[] { "Organization_Id" });
            CreateTable(
                "dbo.OrganizationSituations",
                c => new
                    {
                        Organization_Id = c.Int(nullable: false),
                        Situation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_Id, t.Situation_Id })
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .ForeignKey("dbo.Situations", t => t.Situation_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id)
                .Index(t => t.Situation_Id);
            
            DropColumn("dbo.Situations", "Organization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Situations", "Organization_Id", c => c.Int());
            DropForeignKey("dbo.OrganizationSituations", "Situation_Id", "dbo.Situations");
            DropForeignKey("dbo.OrganizationSituations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.OrganizationSituations", new[] { "Situation_Id" });
            DropIndex("dbo.OrganizationSituations", new[] { "Organization_Id" });
            DropTable("dbo.OrganizationSituations");
            CreateIndex("dbo.Situations", "Organization_Id");
            AddForeignKey("dbo.Situations", "Organization_Id", "dbo.Organizations", "Id");
        }
    }
}
