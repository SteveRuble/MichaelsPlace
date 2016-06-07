namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class people : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.UserModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsDisabled = c.Boolean(nullable: false),
                        Email = c.String(),
                        IsLockedOut = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
