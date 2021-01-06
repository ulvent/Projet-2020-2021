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
    public class ProductsController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Products
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, string searchType, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var Produits = from Produit in db.Produits
                           select Produit;

            if (!String.IsNullOrEmpty(searchString))
            {
                Produits = Produits.Where(s => s.Name_prod.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(searchType))
            {
                if (searchType.Equals("all"))
                {
                    Produits = from Produit in db.Produits
                               select Produit;
                }
                else
                {
                    Produits = Produits.Where(s => s.type.Equals(searchType));
                }
                
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Produits = Produits.OrderByDescending(s => s.Name_prod);
                    break;
                case "Pricc":
                    Produits = Produits.OrderBy(s => s.Price);
                    break;
                case "Price_desc":
                    Produits = Produits.OrderByDescending(s => s.Price);
                    break;
                default:
                    Produits = Produits.OrderBy(s => s.Name_prod);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Produits.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Produits.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_prod,Name_prod,Price,Stock,type")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Produits.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Produits.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_prod,Name_prod,Price,Stock,type")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Produits.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Produits.Find(id);
            db.Produits.Remove(product);
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

        //commande
        public ActionResult Add(int id)
        {
            System.Diagnostics.Debug.WriteLine("add on");
            var test = from Order in db.Commande
                       select Order;
            List<Order> otest = new List<Order>();
            foreach(Order ot in test)
            {
                if (ot.Id_cli == int.Parse(Session["id"].ToString()) && ot.valid == 0)
                {
                    otest.Add(ot);
                }
            }
            if (otest.Count()==0)
            { 
                Order o = new Order { Id_cli = int.Parse(Session["id"].ToString()), valid = 0 };
                db.Commande.Add(o);
                db.SaveChanges();
                Order Order=null;
                foreach (Order or in test)
                {
                    if (or.Id_cli == int.Parse(Session["id"].ToString()) && or.valid == 0)
                    {
                        Order=or;
                    }
                }
                ContainOrder co = new ContainOrder { ID_Comm = Order.ID_comm1, ID_Product = id };
                db.CO.Add(co);
                db.SaveChanges();
            }
            else
            {
                Order Order = null;
                foreach (Order or in test)
                {
                    if (or.Id_cli == int.Parse(Session["id"].ToString()) && or.valid == 0)
                    {
                        Order = or;
                    }
                }
                ContainOrder co = new ContainOrder { ID_Comm = Order.ID_comm1, ID_Product = id };
                db.CO.Add(co);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
