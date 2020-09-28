using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;
using PagedList;

namespace WebAppFirst.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, string ProductCategory, string currentProductCategory, int? page, int? pagesize)
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

                if (searchString1 != null)
                {
                    page = 1;
                }
                else
                {
                    searchString1 = currentFilter1;
                }

                ViewBag.currentFilter1 = searchString1;

                if ((ProductCategory != null) && (ProductCategory != "0"))
                {
                    page = 1;
                }
                else
                {
                    ProductCategory = currentProductCategory;
                }

                ViewBag.currentProductCategory = ProductCategory;

                northwindEntities db = new northwindEntities();

                // tähän tuotteet-olioon voidaan myöhemmin kohdistaa lisää metodeja
                var tuotteet = from p in db.Products
                               select p;

                if (!String.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))
                {
                    int para = int.Parse(ProductCategory);
                    tuotteet = tuotteet.Where(p => p.CategoryID == para);
                }

            // eli jos searchString ei ole tyhjä, sitten tehdään rajaus Where tuotteen nimi sisältää käyttäjän antaman tekstin (searchString1). Sisältää kaikki, joissa kirjoitettu sisältyy koska "contains"
            if (!String.IsNullOrEmpty(searchString1))
                {
                    switch (sortOrder) //Jos hakufiltteri on käytössä, nii käytetään sitä ja lisäksi lajitellaan tulokset
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.UnitPrice);
                            break;
                        case "unitprice_desc":
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1)).OrderBy(p => p.ProductName);
                            break;
                    }
                //tuotteet = tuotteet.Where(p => p.ProductName.Contains(searchString1));
                } //if:n loppu
                else if (!String.IsNullOrEmpty(ProductCategory) && (ProductCategory != "0"))
                {
                    int para = int.Parse(ProductCategory);
                    switch (sortOrder)
                    {
                        case "productname_desc":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderByDescending(p => p.ProductName);
                            break;
                        case "UnitPrice":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderBy(p => p.UnitPrice);
                            break;
                        case "unitprice_desc":
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderByDescending(p => p.UnitPrice);
                            break;
                        default:
                            tuotteet = tuotteet.Where(p => p.CategoryID == para).OrderBy(p => p.ProductName);
                            break;
                    }
                } //else ifin loppu
                else //Tässä hakufiltteri ei ole käytössä, joten lajitellaan koko tulosjoukko ilman suodatuksia
                {
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
                }; //if-elsen loppu

                List<Categories> lstCategories = new List<Categories>();

                var categoryList = from cat in db.Categories
                                   select cat;

                Categories tyhjaCategory = new Categories();
                tyhjaCategory.CategoryID = 0;
                tyhjaCategory.CategoryName = "";
                tyhjaCategory.CategoryIDCategoryName = "";
                lstCategories.Add(tyhjaCategory);

                foreach (Categories category in categoryList)
                {
                    Categories yksiCategory = new Categories();
                    yksiCategory.CategoryID = category.CategoryID;
                    yksiCategory.CategoryName = category.CategoryName;
                    yksiCategory.CategoryIDCategoryName = category.CategoryID.ToString() + " - " + category.CategoryName;
                    lstCategories.Add(yksiCategory);
                }
                ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryIDCategoryName", ProductCategory);

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //Tämä palauttaa sivunumeron tai jos page on null, niin palauttaa numeron yksi       
                // Vanhat hakulauseet:
                //List<Products> model = db.Products.ToList();
                //db.Dispose();
                return View(tuotteet.ToPagedList(pageNumber, pageSize));
            //} //Tämä on if-elsen viimeinen hakasulku
        } //ActionResult Indexin loppu

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