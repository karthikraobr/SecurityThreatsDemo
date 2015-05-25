using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore.Models;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            if (Session["Logged"] == null && Request.Cookies["AuthToken"] == null)
                return RedirectToAction("LogOn", "Account");
            var genres = storeDB.Genres.ToList();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public ActionResult Search(string searchStr)
        {
            DataTable dt = new DataTable();
            string connString = storeDB.Database.Connection.ConnectionString;
           // string query = string.Format("select * from [Album] where Title like '%{0}'", searchStr);
            string query = "select * from [Album] where Title like @TitleName+'%' ";
            string str = searchStr.Substring(0, 3);
            try
            {             
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.Add(new SqlParameter("TitleName", str));
                    // SqlDataAdapter da = cmd.ExecuteReader();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    connection.Close();
                }

            }
            catch (Exception ex)
            {


            }
            ViewBag.Data = dt;
            return View();
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
            if (Session["Logged"] == null && Request.Cookies["AuthToken"] == null)
                return RedirectToAction("LogOn", "Account");
            // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var album = storeDB.Albums.Find(id);
            //throw new ApplicationException("Testing default ErrorHandler attribute");
            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();

            return PartialView(genres);
        }

    }
}