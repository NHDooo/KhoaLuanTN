using Models.Dao;
using Models.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieProject.Common;
namespace MovieProject.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class MovieController : BaseController
    {
        // GET: Admin/Movie
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
           
            List<Category> listcategory = db.Categories.Where(x =>x.Status==true).ToList();
            ViewBag.listcategory = listcategory;
            List<Country> listcountry = db.Countries.Where(x => x.Status == true).ToList();
            ViewBag.listcountry = listcountry;
            var dao = new MovieDao();
            var model = dao.ListM( page, pageSize);
            return View(model);
        }
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Movies.SingleOrDefault(x => x.MovieID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]

        public ActionResult Add(Movie model, string submit)
        {
           
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.Image = model.Image.ToString();
                    model.MoreImages = model.MoreImages.ToString();
                    model.Actor = model.Actor.ToString();
                    model.Description = model.Description.ToString();
                    model.Directors = model.Directors.ToString();
                    model.Time = model.Time.ToString();
                    model.Year = model.Year;
                    //model.Country = model.Country.ToString();
                    model.MovieLink = model.MovieLink.ToString();
                    model.TrailerLink = model.TrailerLink.ToString();
                    model.CategoryID = model.CategoryID;
                    model.Rate = model.Rate;
                    model.Viewed = model.Viewed;
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    model.TopHot = model.TopHot;
                    model.MetaKeywords = StringHelper.ToUnsignString(model.Name);
                    model.CountryID = model.CountryID;
                    db.Movies.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                List<Category> listcategory = db.Categories.ToList();
                ViewBag.listcategory = listcategory;
                List<Country> listcountry = db.Countries.Where(x => x.Status == true).ToList();
                ViewBag.listcountry = listcountry;
                List<Movie> list = GetData();
                ViewBag.list = list;
                SetAlert("Thêm phim thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var listupdate = db.Movies.SingleOrDefault(x => x.MovieID == model.MovieID);
                    listupdate.Name = model.Name;
                    listupdate.Image = model.Image;
                    listupdate.MoreImages = model.MoreImages;
                    listupdate.Actor = model.Actor;
                    listupdate.Description = model.Description;
                    listupdate.Directors = model.Directors;
                    listupdate.Time = model.Time;
                    listupdate.Year = model.Year;
                    //listupdate.Country = model.Country;
                    listupdate.MovieLink = model.MovieLink;
                    listupdate.TrailerLink = model.TrailerLink;
                    listupdate.CategoryID = model.CategoryID;
                    listupdate.Rate = model.Rate;
                    listupdate.Viewed = model.Viewed;
                    listupdate.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    listupdate.Status = model.Status;
                    listupdate.TopHot = model.TopHot;
                    listupdate.MetaKeywords =StringHelper.ToUnsignString(model.Name);
                    listupdate.CountryID = model.CountryID;
                    db.SaveChanges();
                    model = null;
                }
                List<Category> listcategory = db.Categories.ToList();
                ViewBag.listcategory = listcategory;
                List<Country> listcountry = db.Countries.Where(x => x.Status == true).ToList();
                ViewBag.listcountry = listcountry;
                List<Movie> list = GetData();
                ViewBag.list = list;
                SetAlert("Sửa phim thành công", "success");
                return RedirectToAction("Index",list);
            }

            else
            {
                List<Movie> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Movie> GetData()
        {
            return db.Movies.ToList();


        }
        public ActionResult Delete(int id)
        {
            new MovieDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new MovieDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}