namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTagDisplayName : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tags", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "DisplayName", c => c.String());
        }
    }
}
