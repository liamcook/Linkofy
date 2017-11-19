namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class panda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        client = c.String(nullable: false),
                        monthlyQuota = c.Int(nullable: false),
                        TopicTF = c.String(nullable: false),
                        UserTableID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserTables", t => t.UserTableID, cascadeDelete: true)
                .Index(t => t.UserTableID);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkID = c.Int(nullable: false, identity: true),
                        Obdomain = c.String(nullable: false),
                        ClientID = c.Int(),
                        Obpage = c.String(nullable: false),
                        Anchor = c.String(nullable: false),
                        BuildDate = c.DateTime(nullable: false),
                        IdentifierID = c.Int(),
                        UserTableID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LinkID)
                .ForeignKey("dbo.Clients", t => t.ClientID)
                .ForeignKey("dbo.Identifiers", t => t.IdentifierID)
                .ForeignKey("dbo.UserTables", t => t.UserTableID, cascadeDelete: true)
                .Index(t => t.ClientID)
                .Index(t => t.IdentifierID)
                .Index(t => t.UserTableID);
            
            CreateTable(
                "dbo.UserTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userIdentity = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            AddColumn("dbo.Identifiers", "type", c => c.Int());
            AddColumn("dbo.Identifiers", "UserTableID", c => c.Int(nullable: false));
            CreateIndex("dbo.Identifiers", "UserTableID");
            AddForeignKey("dbo.Identifiers", "UserTableID", "dbo.UserTables", "ID", cascadeDelete: true);
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userIdentity = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Clients", "UserTableID", "dbo.UserTables");
            DropForeignKey("dbo.Links", "UserTableID", "dbo.UserTables");
            DropForeignKey("dbo.Links", "IdentifierID", "dbo.Identifiers");
            DropForeignKey("dbo.UserTables", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Identifiers", "UserTableID", "dbo.UserTables");
            DropForeignKey("dbo.Links", "ClientID", "dbo.Clients");
            DropIndex("dbo.UserTables", new[] { "ApplicationUserId" });
            DropIndex("dbo.Identifiers", new[] { "UserTableID" });
            DropIndex("dbo.Links", new[] { "UserTableID" });
            DropIndex("dbo.Links", new[] { "IdentifierID" });
            DropIndex("dbo.Links", new[] { "ClientID" });
            DropIndex("dbo.Clients", new[] { "UserTableID" });
            DropColumn("dbo.Identifiers", "UserTableID");
            DropColumn("dbo.Identifiers", "type");
            DropTable("dbo.UserTables");
            DropTable("dbo.Links");
            DropTable("dbo.Clients");
        }
    }
}
