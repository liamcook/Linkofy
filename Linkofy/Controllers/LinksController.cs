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

namespace Linkofy.Controllers
{
    public class LinksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Links
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            var links = db.Links.Include(l => l.Client).Include(l => l.Identifier).Include(l => l.UserTable);
            return View(links.ToList());
        }

        // GET: Links/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // GET: Links/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN");
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain");
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity");
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkID,Obdomain,ClientID,Obpage,Anchor,BuildDate,IdentifierID,UserTableID")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", link.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", link.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", link.UserTableID);
            return View(link);
        }

        // GET: Links/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", link.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", link.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", link.UserTableID);
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LinkID,Obdomain,ClientID,Obpage,Anchor,BuildDate,IdentifierID,UserTableID")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", link.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", link.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", link.UserTableID);
            return View(link);
        }

        // GET: Links/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Link link = db.Links.Find(id);
            db.Links.Remove(link);
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
