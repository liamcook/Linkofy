namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newclientattr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "webPage", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "contactEmail", c => c.Int(nullable: false));
            AddColumn("dbo.Clients", "contactName", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "contactName");
            DropColumn("dbo.Clients", "contactEmail");
            DropColumn("dbo.Clients", "webPage");
        }
    }
}
