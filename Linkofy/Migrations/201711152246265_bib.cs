namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bib : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MJTopics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        topicalTF = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Clients", "MJTopicsID", c => c.Int());
            AddColumn("dbo.Identifiers", "MJTopicsID", c => c.Int());
            AlterColumn("dbo.Identifiers", "price", c => c.Int(nullable: false));
            CreateIndex("dbo.Clients", "MJTopicsID");
            CreateIndex("dbo.Identifiers", "MJTopicsID");
            AddForeignKey("dbo.Identifiers", "MJTopicsID", "dbo.MJTopics", "ID");
            AddForeignKey("dbo.Clients", "MJTopicsID", "dbo.MJTopics", "ID");
            DropColumn("dbo.Clients", "TopicTF");
            DropColumn("dbo.Identifiers", "TTF");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Identifiers", "TTF", c => c.String());
            AddColumn("dbo.Clients", "TopicTF", c => c.String(nullable: false));
            DropForeignKey("dbo.Clients", "MJTopicsID", "dbo.MJTopics");
            DropForeignKey("dbo.Identifiers", "MJTopicsID", "dbo.MJTopics");
            DropIndex("dbo.Identifiers", new[] { "MJTopicsID" });
            DropIndex("dbo.Clients", new[] { "MJTopicsID" });
            AlterColumn("dbo.Identifiers", "price", c => c.String(nullable: false));
            DropColumn("dbo.Identifiers", "MJTopicsID");
            DropColumn("dbo.Clients", "MJTopicsID");
            DropTable("dbo.MJTopics");
        }
    }
}
