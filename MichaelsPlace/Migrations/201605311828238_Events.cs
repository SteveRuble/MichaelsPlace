namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Events : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Events", newName: "HistoricalEvents");
            AddColumn("dbo.HistoricalEvents", "EventType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HistoricalEvents", "EventType");
            RenameTable(name: "dbo.HistoricalEvents", newName: "Events");
        }
    }
}
