using Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var MovieDao = new MovieDao();
            ViewBag.ListMovieNew = MovieDao.ListMovieNew(12);
            ViewBag.ListMovieTop = MovieDao.ListMovieTop(12);
            ViewBag.ListMoviePo = MovieDao.ListMoviePo(6);
            ViewBag.Slides = new SlideDao().ListAllSlide(5);
            var model = new AdDao().GetContentAd();
            return View(model);
        }

        public ActionResult Search()
        {
            return View();
        }
    }
}