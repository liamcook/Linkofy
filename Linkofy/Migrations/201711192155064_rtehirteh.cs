namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rtehirteh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MajesticDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        ErrorMessage = c.String(),
                        FullError = c.String(),
                        FirstBackLinkDate = c.String(),
                        IndexBuildDate = c.String(),
                        IndexType = c.Int(nullable: false),
                        MostRecentBackLinkDate = c.String(),
                        QueriedRootDomains = c.Int(nullable: false),
                        QueriedSubDomains = c.Int(nullable: false),
                        QueriedURLs = c.Int(nullable: false),
                        QueriedURLsMayExist = c.Int(nullable: false),
                        ServerBuild = c.String(),
                        ServerName = c.String(),
                        ServerVersion = c.String(),
                        UniqueIndexID = c.String(),
                        DataTables_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DataTables", t => t.DataTables_ID)
                .Index(t => t.DataTables_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MajesticDatas", "DataTables_ID", "dbo.DataTables");
            DropIndex("dbo.MajesticDatas", new[] { "DataTables_ID" });
            DropTable("dbo.MajesticDatas");
        }
    }
}
