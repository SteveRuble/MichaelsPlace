namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionPreferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPreferences", "SubscriptionName", c => c.String());
            AddColumn("dbo.Notifications", "ToPhoneNumber", c => c.String());
            DropColumn("dbo.UserPreferences", "EventType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserPreferences", "EventType", c => c.Int());
            DropColumn("dbo.Notifications", "ToPhoneNumber");
            DropColumn("dbo.UserPreferences", "SubscriptionName");
        }
    }
}
