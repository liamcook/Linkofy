namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class potato : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Identifiers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        domain = c.String(nullable: false),
                        contact = c.String(nullable: false),
                        contactname = c.String(),
                        price = c.String(nullable: false),
                        TF = c.Int(nullable: false),
                        CF = c.Int(nullable: false),
                        RI = c.Int(nullable: false),
                        TTF = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Identifiers", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Identifiers", new[] { "ApplicationUserID" });
            DropTable("dbo.Identifiers");
        }
    }
}
