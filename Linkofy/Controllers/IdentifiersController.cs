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
using Linkofy.Functions;
using CsvHelper;
using System.Net.Mail;
using Newtonsoft.Json;
using PagedList;

namespace Linkofy.Controllers
{
    [Authorize]
    public class IdentifiersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Identifiers
        [Authorize]
        public ActionResult Index(string sortOrder, int? page, string currentFilter, string searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DomainSortParm = String.IsNullOrEmpty(sortOrder) ? "Dom_desc" : "";
            ViewBag.ContactESortParm = sortOrder == "ContE" ? "ContE_desc" : "ContE";
            ViewBag.ContNSortParm = sortOrder == "ContN" ? "ContN_desc" : "ContN";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.TFSortParm = sortOrder == "TF" ? "TF_desc" : "TF";
            ViewBag.CFSortParm = sortOrder == "CF" ? "CF_desc" : "CF";
            ViewBag.RISortParm = sortOrder == "RI" ? "RI_desc" : "RI";
            ViewBag.MJSortParm = sortOrder == "MJ" ? "MJ_desc" : "MJ";
            var identifiers = db.Identifiers.Include(i => i.MJTopics).Include(i => i.UserTable);

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
                    identifiers = identifiers.OrderByDescending(s => s.domain);
                    break;
                case "ContE":
                    identifiers = identifiers.OrderBy(s => s.contact);
                    break;
                case "ContE_desc":
                    identifiers = identifiers.OrderByDescending(s => s.contact);
                    break;
                case "ContN":
                    identifiers = identifiers.OrderBy(s => s.contactname);
                    break;
                case "ContN_desc":
                    identifiers = identifiers.OrderByDescending(s => s.contactname);
                    break;
                case "Price":
                    identifiers = identifiers.OrderBy(s => s.price);
                    break;
                case "Price_desc":
                    identifiers = identifiers.OrderByDescending(s => s.price);
                    break;
                case "Type":
                    identifiers = identifiers.OrderBy(s => s.type);
                    break;
                case "Type_desc":
                    identifiers = identifiers.OrderByDescending(s => s.type);
                    break;
                case "TF":
                    identifiers = identifiers.OrderBy(s => s.TrustFlow);
                    break;
                case "TF_desc":
                    identifiers = identifiers.OrderByDescending(s => s.TrustFlow);
                    break;
                case "CF":
                    identifiers = identifiers.OrderBy(s => s.CitationFlow);
                    break;
                case "CF_desc":
                    identifiers = identifiers.OrderByDescending(s => s.CitationFlow);
                    break;
                case "RI":
                    identifiers = identifiers.OrderBy(s => s.RI);
                    break;
                case "RI_desc":
                    identifiers = identifiers.OrderByDescending(s => s.RI);
                    break;
                case "MJ":
                    identifiers = identifiers.OrderBy(s => s.MJTopics.topicalTF);
                    break;
                case "MJ_desc":
                    identifiers = identifiers.OrderByDescending(s => s.MJTopics.topicalTF);
                    break;
                default:
                    identifiers = identifiers.OrderBy(s => s.domain);
                    break;
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            return View(identifiers.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Available(int? id)
        {
            var userId = User.Identity.GetUserId();
            var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
            ViewBag.UserTableID = UserTableID;
            ViewBag.Userid = userId;
            ViewBag.DomainN = db.Identifiers.Where(c => c.ID == id).FirstOrDefault().domain;
            ViewBag.DomainD = id;

            var clients = (from i in db.Clients.Include(i => i.MJTopics).Include(i => i.UserTable).AsQueryable()
                           join l in db.Links.AsQueryable() on new { ID = i.ID, ClientID = id } equals new { ID = l.IdentifierID, ClientID = l.ClientID } into jL
                           where jL.Count() == 0
                           select i);
            return View(clients.ToList());
        }
        // GET: Identifiers/Create
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
                string[] Metric = MajesticFunctions.MajesticChecker(URLtests);
                var userId = User.Identity.GetUserId();
                var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                var identifier = new Identifier { domain = Metric[0], contact = model.contact, contactname = model.contactname.First().ToString().ToUpper() + model.contactname.Substring(1), price = model.price, type = model.type, TrustFlow = Int32.Parse(Metric[1]), CitationFlow = Int32.Parse(Metric[2]), RI = Int32.Parse(Metric[3]), MJTopicsID = model.MJTopicsID, UserTableID = UserTableID };
                ViewBag.identifier = identifier;
                db.Identifiers.Add(identifier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", model.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", model.UserTableID);
            return View(ViewBag.identifier);
        }
        public ActionResult CreateBulk()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CreateBulk")]
        public ActionResult CreateBulkUpload(Identifier model)
        {
            var file = Request.Files["attachmentcsv"];
            using (var csv = new CsvReader(new StreamReader(file.InputStream), true))
            {
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                var records = csv.GetRecords<Identifier>().ToList();
                foreach (var item in records)
                {
                    var strip = item.domain.Replace("https://www.", "").Replace("http://www.", "")
                       .Replace("https://", "").Replace("http://", "").Replace("www.", "");

                    string[] URLtests =
                       {"https://www." + strip, "http://www." + strip, "https://" + strip, "http://" + strip};

                    string[] Metric = MajesticFunctions.MajesticChecker(URLtests);

                    if (!(from c in db.Identifiers
                          select c.domain).Contains(Metric[0]))
                    {
                        var userId = User.Identity.GetUserId();
                        var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;

                        var identifier = new Identifier
                        {
                            domain = Metric[0],
                            contact = item.contact,
                            contactname = item.contactname.First().ToString().ToUpper() + item.contactname.Substring(1),
                            price = item.price,
                            type = item.type,
                            TrustFlow = Int32.Parse(Metric[1]),
                            CitationFlow = Int32.Parse(Metric[2]),
                            RI = Int32.Parse(Metric[3]),
                            MJTopicsID = item.MJTopicsID,
                            UserTableID = UserTableID
                        };
                        ViewBag.identifier = identifier;
                        db.Identifiers.Add(identifier);
                        db.SaveChanges();
                    }
                    else { continue; }
                }
                return RedirectToAction("Index");
            }
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
        public ActionResult Edit(Identifier model)
        {
            if (ModelState.IsValid)
            {
                String Strip = model.domain.Replace("https://www.", "").Replace("http://www.", "").Replace("https://", "").Replace("http://", "").Replace("www.", "");

                string[] URLtests = { "https://www." + Strip, "http://www." + Strip, "https://" + Strip, "http://" + Strip };
                string[] Metric = MajesticFunctions.MajesticChecker(URLtests);
                var userId = User.Identity.GetUserId();
                var UserTableID = db.UserTables.Where(c => c.ApplicationUserId == userId).First().ID;
                var identifier = new Identifier { domain = Metric[0], contact = model.contact, contactname = model.contactname.First().ToString().ToUpper() + model.contactname.Substring(1), price = model.price, type = model.type, TrustFlow = Int32.Parse(Metric[1]), CitationFlow = Int32.Parse(Metric[2]), RI = Int32.Parse(Metric[3]), MJTopicsID = model.MJTopicsID, UserTableID = UserTableID };
                ViewBag.identifier = identifier;
                db.Entry(identifier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MJTopicsID = new SelectList(db.MJTopicss, "ID", "ID", model.MJTopicsID);
            ViewBag.UserTableID = new SelectList(db.UserTables, "ID", "userIdentity", model.UserTableID);
            return View(ViewBag.identifier);
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
        public ActionResult SendMail(int id, string templateName)
        {
            Link link = db.Links.Find(id);
            string type = db.Identifiers.Where(c => c.ID == id).First().type.ToString();
            int templateNUM = db.Templates.Where(c => c.Default.ToString() == type).First().ID;
            ViewBag.subject = db.Templates.Where(c => c.Default.ToString() == type).First().subject;
            ViewBag.body = db.Templates.Where(c => c.Default.ToString() == type).First().Body;
            ViewBag.contactemail = db.Identifiers.Where(c => c.ID == id).First().contact;
            ViewBag.contactname = db.Identifiers.Where(c => c.ID == id).First().contactname;
            ViewBag.emailAddress = new SelectList(db.EmailAccounts, "emailAddress", "emailAddress");
            ViewBag.templateName = new SelectList(db.Templates, "ID", "templateName", templateNUM);

            return View();
        }

        [HttpPost, ActionName("SendMail")]
        public ActionResult SendMailPost(string emailAddress, string toemail, string subject, string body)
        {
            string senderName = db.EmailAccounts.Where(c => c.emailAddress == emailAddress).First().senderName;
            string fromPassword = db.EmailAccounts.Where(c => c.emailAddress == emailAddress).First().password;
            var fromAddress = new MailAddress(emailAddress, senderName);
            var toAddress = new MailAddress(toemail, "Liam Cook");
            

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
                return RedirectToAction("Index");
            }
        }
        public JsonResult TemplateData(int id)
        {
            var v = new {
                subject = db.Templates.Where(c => c.ID == id).First().subject,
                body = db.Templates.Where(c => c.ID == id).First().Body
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
