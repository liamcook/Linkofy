namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ergierv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Data", "Results_ID", "dbo.Results");
            DropForeignKey("dbo.Results", "Headers_ID", "dbo.Headers");
            DropForeignKey("dbo.DataTables", "Results_ID", "dbo.Results");
            DropForeignKey("dbo.MajesticDatas", "DataTables_ID", "dbo.DataTables");
            DropIndex("dbo.DataTables", new[] { "Results_ID" });
            DropIndex("dbo.Results", new[] { "Headers_ID" });
            DropIndex("dbo.Data", new[] { "Results_ID" });
            DropIndex("dbo.MajesticDatas", new[] { "DataTables_ID" });
            DropTable("dbo.DataTables");
            DropTable("dbo.Results");
            DropTable("dbo.Data");
            DropTable("dbo.Headers");
            DropTable("dbo.MajesticDatas");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Headers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaxTopicsRootDomain = c.Int(nullable: false),
                        MaxTopicsSubDomain = c.Int(nullable: false),
                        MaxTopicsURL = c.Int(nullable: false),
                        TopicsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemNum = c.Int(nullable: false),
                        Item = c.String(),
                        ResultCode = c.String(),
                        Status = c.String(),
                        ExtBackLinks = c.Int(nullable: false),
                        RefDomains = c.Int(nullable: false),
                        AnalysisResUnitsCost = c.Int(nullable: false),
                        ACRank = c.Int(nullable: false),
                        ItemType = c.Int(nullable: false),
                        IndexedURLs = c.Int(nullable: false),
                        GetTopBackLinksAnalysisResUnitsCost = c.Int(nullable: false),
                        DownloadBacklinksAnalysisResUnitsCost = c.Int(nullable: false),
                        DownloadRefDomainBacklinksAnalysisResUnitsCost = c.Int(nullable: false),
                        RefIPs = c.Int(nullable: false),
                        RefSubNets = c.Int(nullable: false),
                        RefDomainsEDU = c.Int(nullable: false),
                        ExtBackLinksEDU = c.Int(nullable: false),
                        RefDomainsGOV = c.Int(nullable: false),
                        ExtBackLinksGOV = c.Int(nullable: false),
                        RefDomainsEDU_Exact = c.Int(nullable: false),
                        ExtBackLinksEDU_Exact = c.Int(nullable: false),
                        RefDomainsGOV_Exact = c.Int(nullable: false),
                        ExtBackLinksGOV_Exact = c.Int(nullable: false),
                        CrawledFlag = c.String(),
                        LastCrawlDate = c.String(),
                        LastCrawlResult = c.String(),
                        RedirectFlag = c.String(),
                        FinalRedirectResult = c.String(),
                        OutDomainsExternal = c.String(),
                        OutLinksExternal = c.String(),
                        OutLinksInternal = c.String(),
                        OutLinksPages = c.String(),
                        LastSeen = c.String(),
                        Title = c.String(),
                        RedirectTo = c.String(),
                        Language = c.String(),
                        LanguageDesc = c.String(),
                        LanguageConfidence = c.String(),
                        LanguagePageRatios = c.String(),
                        LanguageTotalPages = c.Int(nullable: false),
                        RefLanguage = c.String(),
                        RefLanguageDesc = c.String(),
                        RefLanguageConfidence = c.String(),
                        RefLanguagePageRatios = c.String(),
                        RefLanguageTotalPages = c.Int(nullable: false),
                        CrawledURLs = c.Int(nullable: false),
                        RootDomainIPAddress = c.String(),
                        TotalNonUniqueLinks = c.String(),
                        NonUniqueLinkTypeHomepages = c.String(),
                        NonUniqueLinkTypeIndirect = c.String(),
                        NonUniqueLinkTypeDeleted = c.String(),
                        NonUniqueLinkTypeNoFollow = c.String(),
                        NonUniqueLinkTypeProtocolHTTPS = c.String(),
                        NonUniqueLinkTypeFrame = c.String(),
                        NonUniqueLinkTypeImageLink = c.String(),
                        NonUniqueLinkTypeRedirect = c.String(),
                        NonUniqueLinkTypeTextLink = c.String(),
                        RefDomainTypeLive = c.String(),
                        RefDomainTypeFollow = c.String(),
                        RefDomainTypeHomepageLink = c.String(),
                        RefDomainTypeDirect = c.String(),
                        RefDomainTypeProtocolHTTPS = c.String(),
                        CitationFlow = c.Int(nullable: false),
                        TrustFlow = c.Int(nullable: false),
                        TrustMetric = c.Int(nullable: false),
                        TopicalTrustFlow_Topic_0 = c.String(),
                        TopicalTrustFlow_Value_0 = c.Int(nullable: false),
                        TopicalTrustFlow_Topic_1 = c.String(),
                        TopicalTrustFlow_Value_1 = c.Int(nullable: false),
                        TopicalTrustFlow_Topic_2 = c.String(),
                        TopicalTrustFlow_Value_2 = c.Int(nullable: false),
                        Results_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Headers_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DataTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Results_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.MajesticDatas", "DataTables_ID");
            CreateIndex("dbo.Data", "Results_ID");
            CreateIndex("dbo.Results", "Headers_ID");
            CreateIndex("dbo.DataTables", "Results_ID");
            AddForeignKey("dbo.MajesticDatas", "DataTables_ID", "dbo.DataTables", "ID");
            AddForeignKey("dbo.DataTables", "Results_ID", "dbo.Results", "ID");
            AddForeignKey("dbo.Results", "Headers_ID", "dbo.Headers", "ID");
            AddForeignKey("dbo.Data", "Results_ID", "dbo.Results", "ID");
        }
    }
}
