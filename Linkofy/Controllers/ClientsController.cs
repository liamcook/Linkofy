using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Linkofy.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace Linkofy.Controllers
{
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            var clients = db.Clients.Include(c => c.MJTopics).Include(c => c.UserTable);
            return View(clients.ToList());
        }
        public ActionResult DetailsView()
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            var identifiers = db.Identifiers.Include(i => i.MJTopics).Include(i => i.UserTable);
            return View(identifiers.ToList());
        }

        // GET: Clients/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "topicalTF");
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            return View();
        }
        public ActionResult Site()
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://orlajames.com");
            // Sends the HttpWebRequest and waits for a response.
            myHttpWebRequest.AllowAutoRedirect = false;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int resulting = (int)myHttpWebResponse.StatusCode;


            return Content(resulting.ToString());
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                String Strip = model.homePage.Replace("https://www.", "").Replace("http://www.", "").Replace("https://", "").Replace("http://", "").Replace("www.","");

                string[] URLtests = { "https://www." + Strip, "http://www." + Strip, "https://" + Strip, "http://" + Strip };

                foreach (string URLt in URLtests)
                {
                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(URLt);  
                    myHttpWebRequest.AllowAutoRedirect = false;
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    int resulting = (int)myHttpWebResponse.StatusCode;
                    if (resulting == 200)
                    {
                        String Urlnew = URLt;
                        ViewBag.FinalURL = URLt.Replace("https://", "").Replace("http://", "");
                            break;
                    }
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.majestic.com/api/json?app_api_key=9852A91EF12A4A3D4DCC7014BD161FF9&cmd=GetIndexItemInfo&items=1&item0=" + ViewBag.FinalURL + "&datasource=fresh");
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        JObject jObject = JObject.Parse(reader.ReadToEnd());
                        JToken Trusty = jObject["DataTables"]["Results"]["Data"][0]["TrustFlow"].Value<int>();
                        JToken City = jObject["DataTables"]["Results"]["Data"][0]["CitationFlow"].Value<int>();
                        JToken RIPy = jObject["DataTables"]["Results"]["Data"][0]["RefIPs"].Value<int>();
                        var userId = User.Identity.GetUserId();
                        var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                        var newclient = new Client { clientN = model.clientN, homePage = ViewBag.FinalURL, clientEmail = model.clientEmail, contName = model.contName.First().ToString().ToUpper() + model.contName.Substring(1), monthlyQuota = model.monthlyQuota, TrustFlow = Int32.Parse(Trusty.ToString()), CitationFlow = Int32.Parse(City.ToString()), RI = Int32.Parse(RIPy.ToString()), MJTopicsID = model.MJTopicsID, UserTableID = UserTableID };
                        ViewBag.newdomain = newclient;
                        db.Clients.Add(newclient);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", model.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", model.UserTableID);
            return View(ViewBag.newdomain);
        }

        // GET: Clients/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "topicalTF", client.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", client.UserTableID);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,clientN,homePage,clientEmail,contName,monthlyQuota,CitationFlow,TrustFlow,MJTopicsID,UserTableID")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", client.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", client.UserTableID);
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
