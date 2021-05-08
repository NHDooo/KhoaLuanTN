using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.EF;
using Models.Dao;
using Newtonsoft.Json;
using PagedList;
using System.Data.Entity;

namespace MovieProject.Controllers
{
    
    public class ClassController : Controller
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly] // hạn chế
        public PartialViewResult Category() // trang the loai
        {
            var model = new CategoryDao().ListAll();
            return PartialView(model);
        }
        [ChildActionOnly] // han che 
        public PartialViewResult Country()
        {
            var model = new CountryDao().ListAll();
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult MenuBottom()
        {
            var model = new CategoryDao().ListAll();
            return PartialView(model);
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Category(long idcate, int page = 1)
        {
            var Movie = new MovieDao();
            var Category = new CategoryDao().ViewDetail(idcate);
            ViewBag.ListMo = Movie.ListMovieNew(12);
            ViewBag.Cate = Category;
            var model = Movie.ListByCateId(idcate);
            return View(model.ToPagedList(page, 12));
        }


        public ActionResult Country(int incon, int page = 1)
        {
            var Movie = new MovieDao();
            var Country = new CountryDao().ViewDetail(incon);
            ViewBag.Country = Country;
            ViewBag.Movie = Movie.ListMovieNew(12);
            var model = Movie.ListByCountryID(incon);
            return View(model.ToPagedList(page, 12));
        }

        public ActionResult MovieDetail(int id)
        {
            var movie = new MovieDao().ViewDetail(id);
            ViewBag.movie = movie;
            ViewBag.category = new CategoryDao().ViewDetail(movie.CategoryID.Value);
            ViewBag.ListRelated = new MovieDao().ListMovieRelated(id, 7);
            ViewBag.ListMoNew = new MovieDao().ListMovieNew1(7);
            Movie upview = db.Movies.Find(id);
            if(upview == null)
            {
                upview.Viewed = 1;
                upview.Actor = upview.Actor;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }
            else
            {
                upview.Viewed = upview.Viewed + 1;
                upview.Name = upview.Name;
                upview.Image = upview.Image;
                upview.MoreImages = upview.MoreImages;
                upview.Actor = upview.Actor;
                upview.Description = upview.Description;
                upview.Directors = upview.Directors;
                upview.Time = upview.Time;
                upview.Year = upview.Year;
                upview.Country = upview.Country;
                upview.MovieLink = upview.MovieLink;
                upview.TrailerLink = upview.TrailerLink;
                upview.CategoryID = upview.CategoryID;
                upview.Rate = upview.Rate;
                upview.CreatedDate = upview.CreatedDate;
                upview.Status = upview.Status;
                upview.TopHot = upview.TopHot;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }



        }
    }
}