using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class SecureController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            if (Session["Logged"] == null)
                return RedirectToAction("LogOn", "Account");

            return View();
        }
    }
}