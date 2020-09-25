using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;

namespace WebAppFirst.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string currentFilter1, string searchString1)
        {
            //if (Session["UserName"] == null)
            //{
            //    return RedirectToAction("login", "home");
            //}
            //else
            //{
                northwindEntities db = new northwindEntities();

                // tähän tuotteet-olioon voidaan myöhemmin kohdistaa lisää metodeja
                var tuotteet = from p in db.Products
                               select p;

                // eli jos searchString ei ole tyhjä, sitten tehdään rajaus Where tuotteen nimi sisältää käyttäjän antaman tekstin (searchString1). Sisältää kaikki, joissa kirjoitettu sisältyy koska "contains"
                if (!String.IsNullOrEmpty(searchString1))
                {
                    tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1));
                }

                // Vanhat hakulauseet:
                //List<Products> model = db.Products.ToList();
                //db.Dispose();
                return View(tuotteet);
            //}
        }

        public ActionResult Index2()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                northwindEntities db = new northwindEntities();
                List<Products> model = db.Products.ToList();
                //db.Dispose();
                return View(model);
            }
        }
    }
}