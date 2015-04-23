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
            var genres = storeDB.Genres.ToList();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(string id)
        {
            TempData["exeption"] = "";
            int id1=1;
            try
            {
                id1 = Int32.Parse(id);
            }
            
            catch(System.Exception e)
            {
                TempData["exeption"]=e.StackTrace.ToString();
                
            }
            var album = storeDB.Albums.Find(id1);
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

        //
        // GET: /Store/Details/5

        public ActionResult Search(string searchStr)
        {
            DataTable dt = new DataTable();
            string connString = storeDB.Database.Connection.ConnectionString;
            string query = string.Format("select * from [Album] where Title like '%{0}'", searchStr);
            //string query = "select * from [Album] where Title = 'Live' OR 1=1";
            try
            {

                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {


            }
            //var Albums = from s in storeDB.Albums
            //             where s.Title.Contains(searchStr)
            //             select s;
            //ViewBag.Albums = Albums.ToList();
            ViewBag.Data = dt;
            return View();
        }
    }
}