using Models.Dao;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    public class NewMController : Controller
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        // GET: New
        public ActionResult Index()
        {
            var dao = new NewDao();
            ViewBag.news = dao.ListNews(9);
            ViewBag.NewsNew = dao.ListNewsNew(5);
            ViewBag.NewsTop = dao.ListNewsTop(6);
          
            
            return View();
        }
        public ActionResult NewDetail(int id)
        {
            var dao = new NewDao();
            ViewBag.news = dao.ViewDetail(id);
            ViewBag.NewsNew = dao.ListNewsNew(5);
            ViewBag.NewsTop = dao.ListNewsTop(6);
            ViewBag.ListMovieNew1 = new MovieDao().ListMovieNew1(12);
            News upview = db.News.Find(id);
            if (upview.Viewed == null)
            {
                upview.Viewed = 1;
                upview.Name = upview.Name;
                upview.Image = upview.Image;
                upview.Description = upview.Description;
                upview.CreatedDate = upview.CreatedDate;
                upview.Status = upview.Status;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }
            else
            {
                upview.Viewed = upview.Viewed + 1;
                upview.Name = upview.Name;
                upview.Image = upview.Image;
                upview.Description = upview.Description;
                upview.CreatedDate = upview.CreatedDate;
                upview.Status = upview.Status;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }
           
        }
    }
}