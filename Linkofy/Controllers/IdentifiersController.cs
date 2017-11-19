using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Linkofy.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Linkofy.Controllers
{
    public class IdentifiersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Identifiers
        public ActionResult Index()
        {
            var identifiers = db.Identifiers.Include(i => i.MJTopics).Include(i => i.UserTable);
            return View(identifiers.ToList());
        }

        // GET: Identifiers/Details/5
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

        // GET: Identifiers/Create
        public ActionResult Create()
        {
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID");
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity");
            return View();
        }

        // POST: Identifiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,domain,contact,contactname,price,type,TF,CF,RI,MJTopicsID,UserTableID")] Identifier identifier)
        {
            if (ModelState.IsValid)
            {
                db.Identifiers.Add(identifier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.majestic.com/api/json?app_api_key=KEY&cmd=GetIndexItemInfo&items=1&item0=http://www.majestic.com&datasource=fresh");
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    string json2 = reader.ReadToEnd();
                    MajesticData r = JsonConvert.DeserializeObject<MajesticData>(json2);
                }
                    ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", identifier.MJTopicsID);
                    ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", identifier.UserTableID);
                    return View(identifier);
                }
            }

        // GET: Identifiers/Edit/5
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
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", identifier.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", identifier.UserTableID);
            return View(identifier);
        }

        // POST: Identifiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,domain,contact,contactname,price,type,TF,CF,RI,MJTopicsID,UserTableID")] Identifier identifier)
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
