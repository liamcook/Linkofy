namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class big : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Identifiers", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Identifiers", new[] { "ApplicationUserID" });
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userIdentity = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Identifiers", "ApplicationUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Identifiers", "ApplicationUserID", c => c.String(maxLength: 128));
            DropTable("dbo.Users");
            CreateIndex("dbo.Identifiers", "ApplicationUserID");
            AddForeignKey("dbo.Identifiers", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
