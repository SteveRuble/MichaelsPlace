namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Addresses", "LineTwo", c => c.String(maxLength: 200));
            DropColumn("dbo.Organizations", "PhaneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Organizations", "PhaneNumber", c => c.String());
            AlterColumn("dbo.Addresses", "LineTwo", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Organizations", "PhoneNumber");
        }
    }
}
