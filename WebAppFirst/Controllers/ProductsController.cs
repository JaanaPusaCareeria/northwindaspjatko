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
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1)
        {
            //if (Session["UserName"] == null)
            //{
            //    return RedirectToAction("login", "home");
            //}
            //else
            //{
                ViewBag.CurrentSort = sortOrder;
                //Tutkii, onko sortOrder tyhjä ja jos se on, se saa arvoksi productname_desc ja jos se ei ole tyhjä (eli siellä on productname_desc, se saa arvoksi tyhjän.
                ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
                //Tämä tutkii samaa kuin yllä oleva UnitPricen kohdalta: eli saa arvoksi joko unitprice_desc tai UnitPrice, riippuen mitä siellä on
                ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "unitprice_desc" : "UnitPrice";

                northwindEntities db = new northwindEntities();

                // tähän tuotteet-olioon voidaan myöhemmin kohdistaa lisää metodeja
                var tuotteet = from p in db.Products
                               select p;

                // eli jos searchString ei ole tyhjä, sitten tehdään rajaus Where tuotteen nimi sisältää käyttäjän antaman tekstin (searchString1). Sisältää kaikki, joissa kirjoitettu sisältyy koska "contains"
                if (!String.IsNullOrEmpty(searchString1))
                {
                    tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1));
                }

                //switch-casella tutkitaan, mikä arvo sortOrderilla on
                switch (sortOrder)
                {
                case "productname_desc":
                    tuotteet = tuotteet.OrderByDescending(p => p.ProductName);
                    break;
                case "UnitPrice":
                    tuotteet = tuotteet.OrderBy(p => p.UnitPrice);
                    break;
                case "unitprice_desc":
                    tuotteet = tuotteet.OrderByDescending(p => p.UnitPrice);
                    break;
                default:
                    tuotteet = tuotteet.OrderBy(p => p.ProductName);
                    break;
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