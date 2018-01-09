using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Linkofy.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNet.Identity;

namespace Linkofy.Controllers
{
    public class IdentifiersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Identifiers
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            var identifiers = db.Identifiers.Include(i => i.MJTopics).Include(i => i.UserTable);
            return View(identifiers.ToList());
        }

        // GET: Identifiers/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identifier identifier = db.Identifiers.Find(id);
            if (identifier == null)
            {
                return HttpNotFound();
            }
            return View(identifier);
        }
        public ActionResult Available(int? id)
        {
            ViewBag.ClientID = id;
            return View();
    }
        // GET: Identifiers/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "topicalTF");
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity");
            return View();
        }

        // POST: Identifiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Identifier model)
        {
            if (ModelState.IsValid)
            {
                String Strip = model.domain.Replace("https://www.", "").Replace("http://www.", "").Replace("https://", "").Replace("http://", "").Replace("www.", "");

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
                        JToken Trusty = jObject["DataTables"]["Results"]["Data"][0]["TrustFlow"].Value<int>() ;
                        JToken City = jObject["DataTables"]["Results"]["Data"][0]["CitationFlow"].Value<int>();
                        JToken RIPy = jObject["DataTables"]["Results"]["Data"][0]["RefIPs"].Value<int>();
                        var userId = User.Identity.GetUserId();
                        var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                        var newdomain = new Identifier { domain = ViewBag.FinalURL, contact = model.contact, contactname = model.contactname.First().ToString().ToUpper() + model.contactname.Substring(1), price = model.price, type = model.type, TrustFlow = Int32.Parse(Trusty.ToString()), CitationFlow = Int32.Parse(City.ToString()), RI = Int32.Parse(RIPy.ToString()), MJTopicsID = model.MJTopicsID, UserTableID = UserTableID };
                        ViewBag.newdomain = newdomain;
                        db.Identifiers.Add(newdomain);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", model.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", model.UserTableID);
            return View(ViewBag.newdomain);
        }

        // GET: Identifiers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identifier identifier = db.Identifiers.Find(id);
            if (identifier == null)
            {
                return HttpNotFound();
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "topicalTF", identifier.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", identifier.UserTableID);
            return View(identifier);
        }

        // POST: Identifiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,domain,contact,contactname,price,type,CitationFlow,TrustFlow,RI,MJTopicsID,ViewBag.UserTableID")] Identifier identifier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(identifier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", identifier.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", identifier.UserTableID);
            return View(identifier);
        }

        // GET: Identifiers/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identifier identifier = db.Identifiers.Find(id);
            if (identifier == null)
            {
                return HttpNotFound();
            }
            return View(identifier);
        }

        // POST: Identifiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Identifier identifier = db.Identifiers.Find(id);
            db.Identifiers.Remove(identifier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details3()
        {
            return View();
        }
        public ActionResult Details2()
        {
            return View();
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
