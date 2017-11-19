namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thumb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusID = c.Int(nullable: false, identity: true),
                        IdentifierID = c.Int(),
                        ClientID = c.Int(),
                        status = c.Int(nullable: false),
                        Last = c.DateTime(nullable: false),
                        UserTableID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StatusID)
                .ForeignKey("dbo.Clients", t => t.ClientID)
                .ForeignKey("dbo.Identifiers", t => t.IdentifierID)
                .ForeignKey("dbo.UserTables", t => t.UserTableID, cascadeDelete: true)
                .Index(t => t.IdentifierID)
                .Index(t => t.ClientID)
                .Index(t => t.UserTableID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Status", "UserTableID", "dbo.UserTables");
            DropForeignKey("dbo.Status", "IdentifierID", "dbo.Identifiers");
            DropForeignKey("dbo.Status", "ClientID", "dbo.Clients");
            DropIndex("dbo.Status", new[] { "UserTableID" });
            DropIndex("dbo.Status", new[] { "ClientID" });
            DropIndex("dbo.Status", new[] { "IdentifierID" });
            DropTable("dbo.Status");
        }
    }
}
