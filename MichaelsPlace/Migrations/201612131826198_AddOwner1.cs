namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwner1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonCases", "Owner", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonCases", "Owner");
        }
    }
}
