using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bladelinkv2.Models;
using bladelinkv2.dal;
using PagedList;

namespace bladelinkv2.Controllers
{
    public class ClientsController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Clients
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.FnameSortParm = sortOrder == "fname" ? "fname_desc" : "fname";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var Client = from client in db.Client
                         select client;
            if (!String.IsNullOrEmpty(searchString))
            {
                Client = Client.Where(s => s.Name.Contains(searchString)
                                       || s.Fname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Client = Client.OrderByDescending(s => s.Name);
                    break;
                case "fname":
                    Client = Client.OrderBy(s => s.Fname);
                    break;
                case "fname_desc":
                    Client = Client.OrderByDescending(s => s.Fname);
                    break;
                default:
                    Client = Client.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Client.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_cli,Name,Fname,Email,Adress,password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_cli,Name,Fname,Email,Adress,password")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
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
            Client client = db.Client.Find(id);
            db.Client.Remove(client);
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
        //get login
        public ActionResult Login()
        {
            return View();
        }

        //post login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String Email,String password)
        {
            if (ModelState.IsValid)
            {
                var data = db.Client.Where(s => s.Email.Equals(Email) && s.password.Equals(password));
                System.Diagnostics.Debug.WriteLine(Email);
                if (data.Count() > 0)
                {
                    System.Diagnostics.Debug.WriteLine("Connection On");
                    Session["id"] = data.FirstOrDefault().ID_cli;
                    Session["name"] = data.FirstOrDefault().Name;
                    Session["Fname"] = data.FirstOrDefault().Fname;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Connection Off");
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login", "Clients");
                }
            }
            return View();
        }


        //logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login","Clients");
        }
    }
}
