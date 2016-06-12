namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoExplicitIsDisabledOnUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "IsDisabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsDisabled", c => c.Boolean(nullable: false));
        }
    }
}
