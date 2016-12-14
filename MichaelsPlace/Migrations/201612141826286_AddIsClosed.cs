namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsClosed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cases", "IsClosed", c => c.Boolean(nullable: false));
            AddColumn("dbo.PersonCases", "IsOwner", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrganizationPersons", "IsOwner", c => c.Boolean(nullable: false));
            DropColumn("dbo.PersonCases", "Owner");
            DropColumn("dbo.OrganizationPersons", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrganizationPersons", "Owner", c => c.Boolean(nullable: false));
            AddColumn("dbo.PersonCases", "Owner", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrganizationPersons", "IsOwner");
            DropColumn("dbo.PersonCases", "IsOwner");
            DropColumn("dbo.Cases", "IsClosed");
        }
    }
}
