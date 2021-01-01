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

namespace bladelinkv2.Controllers
{
    public class ContainOrdersController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: ContainOrders
        public ActionResult Index()
        {
            List<ContainOrder> CO = new List<ContainOrder>();
            List<Product> Prod = new List<Product>();
            var result = from ContainOrders in db.CO
                         select ContainOrders;
            foreach(ContainOrder co in result)
            {
                CO.Add(co);
            }

            var result2 = from Products in db.Produits
                         select Products;
            foreach (Product p in result2)
            {
                Prod.Add(p);
            }
            for(int i=0; i < CO.Count; i++)
            {
                CO[i].p = Prod[i];
            }
            return View(CO.ToList());
        }

        // GET: ContainOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainOrder containOrder = db.CO.Find(id);
            if (containOrder == null)
            {
                return HttpNotFound();
            }
            return View(containOrder);
        }

        // GET: ContainOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContainOrders/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Cont,ID_product,ID_Comm")] ContainOrder containOrder)
        {
            if (ModelState.IsValid)
            {
                db.CO.Add(containOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(containOrder);
        }

        // GET: ContainOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainOrder containOrder = db.CO.Find(id);
            if (containOrder == null)
            {
                return HttpNotFound();
            }
            return View(containOrder);
        }

        // POST: ContainOrders/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Cont,ID_product,ID_Comm")] ContainOrder containOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(containOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(containOrder);
        }

        // GET: ContainOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContainOrder containOrder = db.CO.Find(id);
            if (containOrder == null)
            {
                return HttpNotFound();
            }
            return View(containOrder);
        }

        // POST: ContainOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContainOrder containOrder = db.CO.Find(id);
            db.CO.Remove(containOrder);
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
