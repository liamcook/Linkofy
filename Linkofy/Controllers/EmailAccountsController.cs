using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Linkofy.Models;

namespace Linkofy.Controllers
{
    [Authorize]
    public class EmailAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmailAccounts
        public ActionResult Index()
        {
            var emailAccounts = db.EmailAccounts.Include(e => e.UserTable);
            return View(emailAccounts.ToList());
        }

        // GET: EmailAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailAccount emailAccount = db.EmailAccounts.Find(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }
            return View(emailAccount);
        }

        // GET: EmailAccounts/Create
        public ActionResult Create()
        {
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity");
            return View();
        }

        // POST: EmailAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,senderName,emailAddress,password,UserTableID")] EmailAccount emailAccount)
        {
            if (ModelState.IsValid)
            {
                db.EmailAccounts.Add(emailAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", emailAccount.UserTableID);
            return View(emailAccount);
        }

        // GET: EmailAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailAccount emailAccount = db.EmailAccounts.Find(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", emailAccount.UserTableID);
            return View(emailAccount);
        }

        // POST: EmailAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,senderName,emailAddress,password,UserTableID")] EmailAccount emailAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emailAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", emailAccount.UserTableID);
            return View(emailAccount);
        }

        // GET: EmailAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailAccount emailAccount = db.EmailAccounts.Find(id);
            if (emailAccount == null)
            {
                return HttpNotFound();
            }
            return View(emailAccount);
        }

        // POST: EmailAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailAccount emailAccount = db.EmailAccounts.Find(id);
            db.EmailAccounts.Remove(emailAccount);
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
