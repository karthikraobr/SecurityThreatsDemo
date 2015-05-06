using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class AlbumController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        // GET: Album
        public ActionResult Index()
        {
            return View(storeDB.Artists.ToList());
        }
        [OptionalAuthorize(true)]
        public ActionResult Edit(int id)
        {
            var album = storeDB.Artists.Find(id);
            return View(album);
        }
        [HttpPost]
        public ActionResult Edit(Artist Albums)
        {
            storeDB.Entry(Albums).State = System.Data.EntityState.Modified;
            storeDB.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            storeDB.Dispose();
            base.Dispose(disposing);
        }
    }
}