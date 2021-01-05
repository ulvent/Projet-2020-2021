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
    public class OrdersController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Orders
        public ViewResult Index()
        {
            var result = from Orders in db.Commande
                         select Orders;

            List<Order> Order = new List<Order>();
            List<ContainOrder> CO = new List<ContainOrder>();
            List<Product> prod = new List<Product>();

            foreach (Order o in result)
            {
                if (o.Id_cli == int.Parse(Session["id"].ToString()) && o.valid == 1)
                {
                    Order.Add(o);
                }
            }
            var result2 = from ContainOrder in db.CO
                          select ContainOrder;
            for (int i = 0; i < Order.Count; i++)
            {
                foreach (ContainOrder co in result2)
                {

                    if (co.ID_Comm == Order[i].ID_comm1)
                    {
                        CO.Add(co);
                    }
                }
            }


            var result3 = from Product in db.Produits
                          select Product;
            for (int i = 0; i < CO.Count; i++)
            {
                foreach (Product p in result3)
                {
                    prod.Add(p);
                }
            }

            for (int j = 0; j < CO.Count; j++)
            {
                for (int i = 0; i < CO.Count; i++)
                {
                    if (CO[j].ID_Product == prod[i].ID_prod)
                    {
                        CO[j].p = prod[i];
                    }
                }
            }

            for (int j = 0; j < Order.Count; j++)
            {
                for (int i = 0; i < CO.Count; i++)
                {
                    if (CO[i].ID_Comm == Order[j].ID_comm1)
                    {
                        Order[j].lp.Add(CO[i]);
                    }
                }
            }
            return View(Order.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Commande.Find(id);
            List<ContainOrder> CO = new List<ContainOrder>();
            List<Product> prod = new List<Product>();

            var result2 = db.CO.Where(s => s.ID_Comm == order.ID_comm1);

            foreach (ContainOrder co in result2)
            {
                CO.Add(co);
            }

            var result3 = from Product in db.Produits
                          select Product;
            for (int i = 0; i < CO.Count; i++)
            {
                foreach (Product p in result3)
                {
                    prod.Add(p);
                }
            }

            for (int j = 0; j < CO.Count; j++)
            {
                for (int i = 0; i < prod.Count; i++)
                {
                    if (CO[j].ID_Product == prod[i].ID_prod)
                    {
                        CO[j].p = prod[i];
                    }
                }
            }

            for (int i = 0; i < CO.Count; i++)
            {
                if (CO[i].ID_Comm == order.ID_comm1)
                {
                    order.lp.Add(CO[i]);
                }
            }
            //end
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_comm1,Id_cli")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Commande.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Commande.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_comm1,Id_cli")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Commande.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Commande.Find(id);
            var result2 = from ContainOrder in db.CO
                          select ContainOrder;
            List<ContainOrder> CO = new List<ContainOrder>();
            foreach (ContainOrder co in result2)
            {

                if (co.ID_Comm == order.ID_comm1)
                {
                    CO.Add(co);
                    System.Diagnostics.Debug.WriteLine(co.ID_Comm);
                }
            }
            for (int i = 0; i < CO.Count; i++)
            {
                if (CO[i].ID_Comm == order.ID_comm1)
                {
                    order.lp.Add(CO[i]);
                }
            }
            for (int i = 0; i < order.lp.Count(); i++)
            {
                db.CO.Remove(order.lp[i]);
            }
            db.Commande.Remove(order);
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

        //CADDY
        public ActionResult Caddy()
        {
            var result = from Orders in db.Commande
                         select Orders;

            Order Order = null;
            List<ContainOrder> CO = new List<ContainOrder>();
            List<Product> prod = new List<Product>();

            foreach (Order o in result)
            {
                if (o.Id_cli == int.Parse(Session["id"].ToString()) && o.valid == 0)
                {
                    Order = o;
                    System.Diagnostics.Debug.WriteLine(Order.ID_comm1);
                }
            }
            var result2 = from ContainOrder in db.CO
                          select ContainOrder;
            if (Order != null)
            {
                foreach (ContainOrder co in result2)
                {

                    if (co.ID_Comm == Order.ID_comm1)
                    {
                        CO.Add(co);
                        System.Diagnostics.Debug.WriteLine(co.ID_Comm);
                    }
                }


                var result3 = from Product in db.Produits
                              select Product;
                for (int i = 0; i < CO.Count; i++)
                {
                    foreach (Product p in result3)
                    {
                        prod.Add(p);
                    }
                }

                for (int j = 0; j < CO.Count; j++)
                {
                    for (int i = 0; i < prod.Count; i++)
                    {
                        if (CO[j].ID_Product == prod[i].ID_prod)
                        {
                            CO[j].p = prod[i];
                            System.Diagnostics.Debug.WriteLine(CO[j].p.Name_prod);
                        }
                    }
                }

                for (int i = 0; i < CO.Count; i++)
                {
                    if (CO[i].ID_Comm == Order.ID_comm1)
                    {
                        Order.lp.Add(CO[i]);
                    }
                }
            }


            return View(Order);

        }
        //validate
        public ActionResult Validate(int id)
        {
            Order o = db.Commande.Find(id);
            List<ContainOrder> CO = new List<ContainOrder>();
            List<Product> prod = new List<Product>();
            var result2 = from ContainOrder in db.CO
                          select ContainOrder;
                foreach (ContainOrder co in result2)
                {

                    if (co.ID_Comm == o.ID_comm1)
                    {
                        CO.Add(co);
                        System.Diagnostics.Debug.WriteLine(co.ID_Comm);
                    }
                }


                var result3 = from Product in db.Produits
                              select Product;
                for (int i = 0; i < CO.Count; i++)
                {
                    foreach (Product p in result3)
                    {
                        prod.Add(p);
                    }
                }

                for (int j = 0; j < CO.Count; j++)
                {
                    for (int i = 0; i < prod.Count; i++)
                    {
                        if (CO[j].ID_Product == prod[i].ID_prod)
                        {
                           prod[i].Stock-=1;
                        db.Entry(prod[i]).State = EntityState.Modified;
                    }
                    }
                }
             o.valid = 1;
            db.Entry(o).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
