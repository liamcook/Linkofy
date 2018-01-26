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
using LINQtoCSV;
using System.Configuration;
using System.Data.OleDb;
using System.Data.Common;
using CsvHelper;
using Linkofy.Functions;
using PagedList;


namespace Linkofy.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, string NameString, string TopicString, string RIString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.HpageSortParm = sortOrder == "Hpage" ? "Hpage_desc" : "Hpage";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.contNameSortParm = sortOrder == "contName" ? "contName_desc" : "contName";
            ViewBag.QTASortParm = sortOrder == "QTA" ? "QTA_desc" : "QTA";
            ViewBag.TrustFlowSortParm = sortOrder == "TF" ? "TF_desc" : "TF";
            ViewBag.CFSortParm = sortOrder == "CF" ? "CF_desc" : "CF";
            ViewBag.RISortParm = sortOrder == "RI" ? "RI_desc" : "RI";
            ViewBag.MJSortParm = sortOrder == "MJ" ? "MJ_desc" : "MJ";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var clients = from s in db.Clients.Include(c => c.MJTopics).Include(c => c.UserTable)
            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.clientN.Contains(searchString)
                                       || s.homePage.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(NameString))
            {
                clients = clients.Where(s => s.clientEmail.Contains(NameString)
                                       || s.contName.Contains(NameString));
            }
            if (!String.IsNullOrEmpty(RIString))
            {
                clients = clients.Where(s => s.clientN.Contains(RIString));
            }
            if (!String.IsNullOrEmpty(TopicString))
            {
                clients = clients.Where(s => s.MJTopics.topicalTF.Contains(TopicString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.clientN);
                    break;
                case "Hpage":
                    clients = clients.OrderBy(s => s.homePage);
                    break;
                case "Hpage_desc":
                    clients = clients.OrderByDescending(s => s.homePage);
                    break;
                case "Email":
                    clients = clients.OrderBy(s => s.clientEmail);
                    break;
                case "Email_desc":
                    clients = clients.OrderByDescending(s => s.clientEmail);
                    break;
                case "contName":
                    clients = clients.OrderBy(s => s.contName);
                    break;
                case "contName_desc":
                    clients = clients.OrderByDescending(s => s.contName);
                    break;
                case "QTA":
                    clients = clients.OrderBy(s => s.monthlyQuota);
                    break;
                case "QTA_desc":
                    clients = clients.OrderByDescending(s => s.monthlyQuota);
                    break;
                case "TF":
                    clients = clients.OrderBy(s => s.TrustFlow);
                    break;
                case "TF_desc":
                    clients = clients.OrderByDescending(s => s.TrustFlow);
                    break;
                case "CF":
                    clients = clients.OrderBy(s => s.CitationFlow);
                    break;
                case "CF_desc":
                    clients = clients.OrderByDescending(s => s.CitationFlow);
                    break;
                case "RI":
                    clients = clients.OrderBy(s => s.RI);
                    break;
                case "RI_desc":
                    clients = clients.OrderByDescending(s => s.RI);
                    break;
                case "MJ":
                    clients = clients.OrderBy(s => s.MJTopics.topicalTF);
                    break;
                case "MJ_desc":
                    clients = clients.OrderByDescending(s => s.MJTopics.topicalTF);
                    break;
                default:
                    clients = clients.OrderBy(s => s.clientN);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            return View(clients.ToPagedList(pageNumber, pageSize));
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
            ViewBag.ClientID = id;
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
        public ActionResult Available(int? id)
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            ViewBag.ClientN = db.Clients.Where(c => c.ID == id).FirstOrDefault().clientN;
            ViewBag.ClientD = id;

            var identifiers = (from i in db.Identifiers.Include(i => i.MJTopics).Include(i => i.UserTable).AsQueryable()
                               join l in db.Links.AsQueryable() on new { ID = i.ID, ClientID = id } equals new { ID = l.IdentifierID, ClientID = l.ClientID } into jL
                               where jL.Count() == 0
                               select i);
            return View(identifiers.ToList());
        }
        public ActionResult CreateBulk()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client model)
        {
            if (ModelState.IsValid)
            {
                String Strip = model.homePage.Replace("https://www.", "").Replace("http://www.", "").Replace("https://", "").Replace("http://", "").Replace("www.", "");
                string[] URLtests = { "https://www." + Strip, "http://www." + Strip, "https://" + Strip, "http://" + Strip };
                    string[] Metric = MajesticFunctions.MajesticChecker(URLtests);
                    var userId = User.Identity.GetUserId();
                    var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                    var newclient = new Client { clientN = model.clientN, homePage = Metric[0], clientEmail = model.clientEmail, contName = model.contName.First().ToString().ToUpper() + model.contName.Substring(1), monthlyQuota = model.monthlyQuota, TrustFlow = Int32.Parse(Metric[1]), CitationFlow = Int32.Parse(Metric[2]), RI = Int32.Parse(Metric[3]), MJTopicsID = model.MJTopicsID, UserTableID = UserTableID };
                    ViewBag.newdomain = newclient;
                    db.Clients.Add(newclient);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", model.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", model.UserTableID);
            return View(ViewBag.newdomain);
        }
        [HttpPost]
        [ActionName("CreateBulk")]
        public ActionResult CreateBulkUpload(Client model)
        {
            var file = Request.Files["attachmentcsv"];
            using (var csv = new CsvReader(new StreamReader(file.InputStream), true))
            {
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                var records = csv.GetRecords<Client>().ToList();
                foreach (var item in records)
                {
                    if (!(from c in db.Clients
                          select c.clientN).Contains(item.clientN))
                    {
                        var strip = item.homePage.Replace("https://www.", "").Replace("http://www.", "")
                            .Replace("https://", "").Replace("http://", "").Replace("www.", "");

                        string[] URLtests =
                            {"https://www." + strip, "http://www." + strip, "https://" + strip, "http://" + strip};

                        string[] Metric = MajesticFunctions.MajesticChecker(URLtests);
                        var userId = User.Identity.GetUserId();
                        var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;

                        var newclient = new Client

                        {
                            clientN = item.clientN,
                            homePage = Metric[0],
                            clientEmail = item.clientEmail,
                            contName = item.contName,
                            monthlyQuota = item.monthlyQuota,
                            TrustFlow = Int32.Parse(Metric[1]),
                            CitationFlow = Int32.Parse(Metric[2]),
                            RI = Int32.Parse(Metric[3]),
                            MJTopicsID = item.MJTopicsID,
                            UserTableID = UserTableID
                        };
                        db.Clients.Add(newclient);
                        db.SaveChanges();
                    }
                    else { continue; }
                }
                    return RedirectToAction("Index");
            }
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
