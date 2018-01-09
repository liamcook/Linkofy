namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ierg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "homePage", c => c.String());
            AddColumn("dbo.Clients", "clientEmail", c => c.String());
            AddColumn("dbo.Clients", "contName", c => c.String());
            DropColumn("dbo.Clients", "webPage");
            DropColumn("dbo.Clients", "contactEmail");
            DropColumn("dbo.Clients", "contactName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "contactName", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "contactEmail", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "webPage", c => c.Int(nullable: false));
            DropColumn("dbo.Clients", "contName");
            DropColumn("dbo.Clients", "clientEmail");
            DropColumn("dbo.Clients", "homePage");
        }
    }
}
