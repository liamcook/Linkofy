namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ergerg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "RI", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "RI");
        }
    }
}
