using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bladelinkv2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Session["id"] = 1;
            //Session["Nom"] = "Callens";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}