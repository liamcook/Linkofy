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
    public class StatusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Status
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            var statuss = db.Statuss.Include(s => s.Client).Include(s => s.Identifier).Include(s => s.UserTable);
            return View(statuss.ToList());
        }

        // GET: Status/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Statuss.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // GET: Status/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN");
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain");
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity");
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusID,IdentifierID,ClientID,status,Last,UserTableID")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Statuss.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", status.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", status.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", status.UserTableID);
            return View(status);
        }

        // GET: Status/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Statuss.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", status.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", status.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", status.UserTableID);
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusID,IdentifierID,ClientID,status,Last,UserTableID")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", status.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", status.IdentifierID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", status.UserTableID);
            return View(status);
        }

        // GET: Status/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Statuss.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status status = db.Statuss.Find(id);
            db.Statuss.Remove(status);
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
