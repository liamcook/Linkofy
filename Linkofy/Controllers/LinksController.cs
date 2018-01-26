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
using Linkofy.Functions;
using CsvHelper;
using System.IO;
using PagedList;
using Newtonsoft.Json;

namespace Linkofy.Controllers
{
    [Authorize]
    public class LinksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Links
        [Authorize]
        public ActionResult Index(string sortOrder, int? page, string searchString, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DomainSortParm = String.IsNullOrEmpty(sortOrder) ? "Dom_desc" : "";
            ViewBag.LinkSortParm = sortOrder == "Link" ? "Link_desc" : "Link";
            ViewBag.ClientSortParm = sortOrder == "Clie" ? "Clie_desc" : "Clie";
            ViewBag.OBSortParm = sortOrder == "OB" ? "OB_desc" : "OB";
            ViewBag.AnchorSortParm = sortOrder == "Anc" ? "Anc_desc" : "Anc";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.BuildSortParm = sortOrder == "Buil" ? "Buil_desc" : "Buil";
            var links = db.Links.Include(l => l.Client).Include(l => l.Identifier).Include(l => l.UserTable);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "Dom_desc":
                    links = links.OrderByDescending(s => s.Identifier.domain);
                    break;
                case "Link":
                    links = links.OrderBy(s => s.Obdomain);
                    break;
                case "Link_desc":
                    links = links.OrderByDescending(s => s.Obdomain);
                    break;
                case "Clie":
                    links = links.OrderBy(s => s.Client.clientN);
                    break;
                case "Clie_desc":
                    links = links.OrderByDescending(s => s.Client.clientN);
                    break;
                case "OB":
                    links = links.OrderBy(s => s.Obpage);
                    break;
                case "OB_desc":
                    links = links.OrderByDescending(s => s.Obpage);
                    break;
                case "Anc":
                    links = links.OrderBy(s => s.Anchor);
                    break;
                case "Anc_desc":
                    links = links.OrderByDescending(s => s.Anchor);
                    break;
                case "Type":
                    links = links.OrderBy(s => s.Identifier.type);
                    break;
                case "Type_desc":
                    links = links.OrderByDescending(s => s.Identifier.type);
                    break;
                case "Buil":
                    links = links.OrderBy(s => s.BuildDate);
                    break;
                case "Buil_desc":
                    links = links.OrderByDescending(s => s.BuildDate);
                    break;
                default:
                    links = links.OrderBy(s => s.Identifier.domain);
                    break;
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            return View(links.ToPagedList(pageNumber, pageSize));
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
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.ClientID = new SelectList(db.Clients.Where(c => c.UserTableID == UserTableID), "ID", "clientN");
            ViewBag.IdentifierID = new SelectList(db.Identifiers.Where(c => c.UserTableID == UserTableID), "ID", "domain");
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Link model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;

                Uri Obdomain = new Uri(model.Obdomain);
                string baseUri = Obdomain.GetLeftPart(System.UriPartial.Authority);
                Uri Obpage = new Uri(model.Obpage);
                string basiUri = Obpage.GetLeftPart(System.UriPartial.Authority);

                string pageContent = null;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(baseUri + model.Obdomain.Replace(baseUri, ""));
                HttpWebResponse myres = (HttpWebResponse)myReq.GetResponse();

                using (StreamReader sr = new StreamReader(myres.GetResponseStream()))
                {
                    pageContent = sr.ReadToEnd();
                }

                string live = "";
                if (pageContent.Contains(model.Obpage))
                {
                    live = "Yes";
                }
                else { live = "No"; }

                var link = new Link { Obdomain = model.Obdomain.Replace(baseUri,""), ClientID = model.ClientID, Obpage = model.Obpage.Replace(basiUri, ""), BuildDate = model.BuildDate, Anchor = model.Anchor, IdentifierID = model.IdentifierID, live = (Link.Live)Enum.Parse(typeof(Link.Live), live), UserTableID = UserTableID };
                ViewBag.link = link;
                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ID", "clientN", model.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers, "ID", "domain", model.IdentifierID);
            return View(ViewBag.link);
        }
        public ActionResult CreateBulk()
        {
            return View();
        }
        [HttpPost]
        [ActionName("CreateBulk")]
        public ActionResult CreateBulkUpload(Link model)
        {
            var file = Request.Files["attachmentcsv"];
            using (var csv = new CsvReader(new StreamReader(file.InputStream), true))
            {
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                var records = csv.GetRecords<Link>().ToList();
                foreach (var item in records)
                {
                    if (!(from c in db.Links
                          select c.Obdomain).Contains(item.Obdomain) && !(from c in db.Links select c.Obpage).Contains(item.Obpage))
                    {
                        var userId = User.Identity.GetUserId();
                        var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                        Uri Obdomain = new Uri(item.Obdomain);
                        string baseUri = Obdomain.GetLeftPart(System.UriPartial.Authority);
                        Uri Obpage = new Uri(item.Obpage);
                        string basiUri = Obpage.GetLeftPart(System.UriPartial.Authority);
                        int domainID = db.Identifiers.Where(c => c.domain.Contains(baseUri.Substring(8, 14))).First().ID;
                        int clientID = db.Clients.Where(c => c.homePage.Contains(basiUri.Substring(8, 14))).First().ID;

                        var link = new Link
                        {
                            Obdomain = item.Obdomain.Replace(baseUri,""),
                            Obpage = item.Obpage.Replace(baseUri, ""),
                            BuildDate = item.BuildDate,
                            Anchor = item.Anchor,
                            IdentifierID = domainID,
                            ClientID = clientID,
                            UserTableID = UserTableID
                        };
                    ViewBag.link = link;
                        db.Links.Add(link);
                        db.SaveChanges();
                    }
                    else { continue; }
                }
                return RedirectToAction("Index");
            }
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
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.ClientID = new SelectList(db.Clients.Where(c => c.UserTableID == UserTableID), "ID", "clientN", link.ClientID);
            ViewBag.IdentifierID = new SelectList(db.Identifiers.Where(c => c.UserTableID == UserTableID), "ID", "domain", link.IdentifierID);
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
        public ActionResult CheckLinks(Link model)
        {
                var userId = User.Identity.GetUserId();
                var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                var items = db.Links.Where(p => p.UserTable.ID == UserTableID).ToList();
                foreach (var item in items)
                {
                    string pageContent = null;

                    string visiturl = "http://" + item.Identifier.domain + item.Obdomain;
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(visiturl);
                    HttpWebResponse myres = (HttpWebResponse)myReq.GetResponse();

                    using (StreamReader sr = new StreamReader(myres.GetResponseStream()))
                    {
                        pageContent = sr.ReadToEnd();
                    }
                    string live = "";
                    if (pageContent.Contains(item.Obpage))
                    {
                        live = "Yes";
                    }
                    else { live = "Yes"; }

                    var link = db.Links.SingleOrDefault(c => c.LinkID == item.LinkID);
                    link.live = (Link.Live)Enum.Parse(typeof(Link.Live), live);
                        db.Entry(link).State = EntityState.Modified;
                        db.SaveChanges();
                    }
            return RedirectToAction("Index");
        }
        public JsonResult ObDo(string urllink)
        {
            string newurl = urllink.Replace("Https://", "").Replace("http://", "").Replace("/*", "");
            var v = new
            {
            domain = db.Identifiers.Where(c => c.domain.Contains(newurl)).First().ID
            };
            String json = JsonConvert.SerializeObject(v);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObPa(string urlpg)
        {
            string newurl = urlpg.Replace("Https://", "").Replace("http://", "").Replace("/*", "");
            var v = new
            {
                client = db.Clients.Where(c => c.homePage.Contains(newurl)).First().ID
            };
            String json = JsonConvert.SerializeObject(v);
            return Json(json, JsonRequestBehavior.AllowGet);
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
