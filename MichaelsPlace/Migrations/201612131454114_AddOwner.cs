namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrganizationPersons", "Owner", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrganizationPersons", "Owner");
        }
    }
}
