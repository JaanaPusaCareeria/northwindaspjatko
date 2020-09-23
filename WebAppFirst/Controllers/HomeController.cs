using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppFirst.Models;

namespace WebAppFirst.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Out";
            }
            else ViewBag.LoggedStatus = "In";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            northwindEntities db = new northwindEntities();
            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ-kyselyllä
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successful login";
                ViewBag.LoggedStatus = "In";
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home"); //Määritellään, mihin onnistunut kirjautuminen johtaa: Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessful";
                ViewBag.LoggedStatus = "Out";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana";
                return View("Login", LoginModel); //epäonnistuneen kirjautumisen jälkeen palataan Login-näkymään
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //uloskirjautumisen jälkeen pääsivulle
        }

        public ActionResult About()
        {
            ViewBag.Message = "Yhtiön perustietojen kuvailua";
            ViewBag.Herja = "Ole huolellinen, niin ei tule virhettä"; //Herja-ominaisuutta ei ole määritelty missään ennakkoon

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Yhteystietojamme";

            return View();
        }


        public ActionResult Map()
        {
            ViewBag.Message = "Saapumisohje";

            return View();
        }
    }
}