using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
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
       // [OptionalAuthorize(true)]
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            var album = storeDB.Artists.Find(id);
            return View(album);
        }

        public ActionResult Export()
        {
            var exportList = storeDB.Artists.ToList();
            GridView gv=new GridView();
            gv.DataSource = storeDB.Artists.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment ; filename = Book1.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw=new StringWriter();
            HtmlTextWriter htw=new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("StudentDetails");
          //  return View(exportList);
           //  return new RazorPDF.PdfResult(exportList, "Export");
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