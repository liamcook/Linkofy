using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Linkofy.Models
{
        public class Headers
        {
            public int ID { get; set; }
        public int MaxTopicsRootDomain { get; set; }
            public int MaxTopicsSubDomain { get; set; }
            public int MaxTopicsURL { get; set; }
            public int TopicsCount { get; set; }
        }

        public class Datum
        {
        public int ID { get; set; }
        public int ItemNum { get; set; }
            public string Item { get; set; }
            public string ResultCode { get; set; }
            public string Status { get; set; }
            public int ExtBackLinks { get; set; }
            public int RefDomains { get; set; }
            public int AnalysisResUnitsCost { get; set; }
            public int ACRank { get; set; }
            public int ItemType { get; set; }
            public int IndexedURLs { get; set; }
            public int GetTopBackLinksAnalysisResUnitsCost { get; set; }
            public int DownloadBacklinksAnalysisResUnitsCost { get; set; }
            public int DownloadRefDomainBacklinksAnalysisResUnitsCost { get; set; }
            public int RefIPs { get; set; }
            public int RefSubNets { get; set; }
            public int RefDomainsEDU { get; set; }
            public int ExtBackLinksEDU { get; set; }
            public int RefDomainsGOV { get; set; }
            public int ExtBackLinksGOV { get; set; }
            public int RefDomainsEDU_Exact { get; set; }
            public int ExtBackLinksEDU_Exact { get; set; }
            public int RefDomainsGOV_Exact { get; set; }
            public int ExtBackLinksGOV_Exact { get; set; }
            public string CrawledFlag { get; set; }
            public string LastCrawlDate { get; set; }
            public string LastCrawlResult { get; set; }
            public string RedirectFlag { get; set; }
            public string FinalRedirectResult { get; set; }
            public string OutDomainsExternal { get; set; }
            public string OutLinksExternal { get; set; }
            public string OutLinksInternal { get; set; }
            public string OutLinksPages { get; set; }
            public string LastSeen { get; set; }
            public string Title { get; set; }
            public string RedirectTo { get; set; }
            public string Language { get; set; }
            public string LanguageDesc { get; set; }
            public string LanguageConfidence { get; set; }
            public string LanguagePageRatios { get; set; }
            public int LanguageTotalPages { get; set; }
            public string RefLanguage { get; set; }
            public string RefLanguageDesc { get; set; }
            public string RefLanguageConfidence { get; set; }
            public string RefLanguagePageRatios { get; set; }
            public int RefLanguageTotalPages { get; set; }
            public int CrawledURLs { get; set; }
            public string RootDomainIPAddress { get; set; }
            public string TotalNonUniqueLinks { get; set; }
            public string NonUniqueLinkTypeHomepages { get; set; }
            public string NonUniqueLinkTypeIndirect { get; set; }
            public string NonUniqueLinkTypeDeleted { get; set; }
            public string NonUniqueLinkTypeNoFollow { get; set; }
            public string NonUniqueLinkTypeProtocolHTTPS { get; set; }
            public string NonUniqueLinkTypeFrame { get; set; }
            public string NonUniqueLinkTypeImageLink { get; set; }
            public string NonUniqueLinkTypeRedirect { get; set; }
            public string NonUniqueLinkTypeTextLink { get; set; }
            public string RefDomainTypeLive { get; set; }
            public string RefDomainTypeFollow { get; set; }
            public string RefDomainTypeHomepageLink { get; set; }
            public string RefDomainTypeDirect { get; set; }
            public string RefDomainTypeProtocolHTTPS { get; set; }
            public int CitationFlow { get; set; }
            public int TrustFlow { get; set; }
            public int TrustMetric { get; set; }
            public string TopicalTrustFlow_Topic_0 { get; set; }
            public int TopicalTrustFlow_Value_0 { get; set; }
            public string TopicalTrustFlow_Topic_1 { get; set; }
            public int TopicalTrustFlow_Value_1 { get; set; }
            public string TopicalTrustFlow_Topic_2 { get; set; }
            public int TopicalTrustFlow_Value_2 { get; set; }
        }

        public class Results
        {
        public int ID { get; set; }
        public Headers Headers { get; set; }
            public List<Datum> Data { get; set; }
        }

        public class DataTables
        {
        public int ID { get; set; }
        public Results Results { get; set; }
        }

        public class MajesticData
        {
        public int ID { get; set; }

        public string Code { get; set; }
            public string ErrorMessage { get; set; }
            public string FullError { get; set; }
            public string FirstBackLinkDate { get; set; }
            public string IndexBuildDate { get; set; }
            public int IndexType { get; set; }
            public string MostRecentBackLinkDate { get; set; }
            public int QueriedRootDomains { get; set; }
            public int QueriedSubDomains { get; set; }
            public int QueriedURLs { get; set; }
            public int QueriedURLsMayExist { get; set; }
            public string ServerBuild { get; set; }
            public string ServerName { get; set; }
            public string ServerVersion { get; set; }
            public string UniqueIndexID { get; set; }
            public DataTables DataTables { get; set; }
        }
    }
