using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class DemoController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public bool UserData(string FirstName, string LastName)
        {
            var user = new UserDetail();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.PhoneNumber = 9976;
            storeDB.UserDetails.Add(user);
            storeDB.SaveChanges();
            return true;
        }
    }
}