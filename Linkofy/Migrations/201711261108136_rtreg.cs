namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rtreg : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Links", "IdentifierID", "dbo.Identifiers");
            DropForeignKey("dbo.Links", "UserTableID", "dbo.UserTables");
            DropIndex("dbo.Links", new[] { "IdentifierID" });
            DropIndex("dbo.Links", new[] { "UserTableID" });
            AlterColumn("dbo.Links", "IdentifierID", c => c.Int(nullable: false));
            AlterColumn("dbo.Links", "UserTableID", c => c.Int());
            CreateIndex("dbo.Links", "IdentifierID");
            CreateIndex("dbo.Links", "UserTableID");
            AddForeignKey("dbo.Links", "IdentifierID", "dbo.Identifiers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Links", "UserTableID", "dbo.UserTables", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Links", "UserTableID", "dbo.UserTables");
            DropForeignKey("dbo.Links", "IdentifierID", "dbo.Identifiers");
            DropIndex("dbo.Links", new[] { "UserTableID" });
            DropIndex("dbo.Links", new[] { "IdentifierID" });
            AlterColumn("dbo.Links", "UserTableID", c => c.Int(nullable: false));
            AlterColumn("dbo.Links", "IdentifierID", c => c.Int());
            CreateIndex("dbo.Links", "UserTableID");
            CreateIndex("dbo.Links", "IdentifierID");
            AddForeignKey("dbo.Links", "UserTableID", "dbo.UserTables", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Links", "IdentifierID", "dbo.Identifiers", "ID");
        }
    }
}
