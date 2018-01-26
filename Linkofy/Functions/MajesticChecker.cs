using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Linkofy.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNet.Identity;


namespace Linkofy.Functions
{
    public class MajesticFunctions
    {
        public static string[] MajesticChecker(string[] URLtests)
        {
                String FinalURL = "";
                foreach (string URLt in URLtests)
                {
                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(URLt);
                    myHttpWebRequest.AllowAutoRedirect = false;
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
    ((sender, certificate, chain, sslPolicyErrors) => true);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    int resulting = (int)myHttpWebResponse.StatusCode;
                    if (resulting == 200)
                    {
                        String Urlnew = URLt;
                        FinalURL = URLt.Replace("https://", "").Replace("http://", "");
                        break;
                    }
                    else
                {

                }
                }

            if (FinalURL.Length > 0)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.majestic.com/api/json?app_api_key=9852A91EF12A4A3D4DCC7014BD161FF9&cmd=GetIndexItemInfo&items=1&item0=" + FinalURL + "&datasource=fresh");
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        JObject jObject = JObject.Parse(reader.ReadToEnd());
                        JToken Trusty = jObject["DataTables"]["Results"]["Data"][0]["TrustFlow"].Value<int>();
                        JToken City = jObject["DataTables"]["Results"]["Data"][0]["CitationFlow"].Value<int>();
                        JToken RIPy = jObject["DataTables"]["Results"]["Data"][0]["RefIPs"].Value<int>();

                        string Trustflow = Trusty.ToString();
                        string Citationflow = City.ToString();
                        string Reffering = RIPy.ToString();

                        String[] Metric = new string[] {FinalURL, Trustflow, Citationflow, Reffering };

                        return Metric;
                    }
                }
            }
            else
            {
                throw new Exception("No Final URL Found");
            }

                }
            }
    }
