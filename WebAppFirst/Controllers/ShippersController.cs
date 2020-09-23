using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;

namespace WebAppFirst.Controllers
{
    public class ShippersController : Controller
    {
        // GET: Shippers

        northwindEntities db = new northwindEntities();
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                var shippers = db.Shippers.Include(s => s.Region); //liitetään Region-taulun tiedot mukaan tähän
                return View(shippers.ToList());

                //List<Shippers> model = db.Shippers.ToList();
                //return View(model);
            }
        }

        private ActionResult View(Func<List<Shippers>> toList)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Edit(int? id) //tämä metodi etsii id:n perusteella oikean shipperin ja alempi etsii mitä kenttiä siellä on.
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //jos id:tä ei ole annettu eli se on null, palautetaan BadRequest
                Shippers shippers = db.Shippers.Find(id); //Shippers-luokan ilmentymä shippers. Etsitään, löytyykö tietokannan Shippers-taulusta parametrina annettu id. Sellaista shipperiä, jonka primary keyssä on tuo tieto.
                if (shippers == null) return HttpNotFound(); //jos sitä ei löydy, palautetaan HttpNotFound
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription", shippers.RegionID);
                return View(shippers); //jos se löytyy, palautetaan näkymänä se shippers.
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShipperID, CompanyName, Phone, RegionID")] Shippers shipper) //haetaan ShipperID jne kentät...? tämä jäi vähän hämäräksi
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipper).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription", shipper.RegionID);
                return RedirectToAction("Index");
            }
            return View(shipper);
        }

        public ActionResult Create()
        {
            //if (Session["UserName"] == null)
            //{
            //    return RedirectToAction("login", "home");
            //}
            //else
            //{
                ViewBag.RegionID = new SelectList(db.Region, "RegionID", "RegionDescription");
                return View();
            //}
        }

        [HttpPost] //create.cshtml-tiedoston painikkeen "submit" generoi HttpPost:n tänne controllerin puolelle
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "ShipperID,CompanyName,Phone")] Shippers shipper) //eli editointinäytöltä tulee dataa ja tässä kerrotaan, mitkä kentät halutaan ottaa mukaan tänne kontrollerin käsittelyyn 
        {
            if (ModelState.IsValid)
            {
                db.Shippers.Add(shipper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipper);
        }

        public ActionResult Delete(int? id) //samanlainen, kuin Edit-metodin alkuosa
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "home");
            }
            else
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Shippers shippers = db.Shippers.Find(id);
                if (shippers == null) return HttpNotFound();
                return View(shippers);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shippers shippers = db.Shippers.Find(id);
            db.Shippers.Remove(shippers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}