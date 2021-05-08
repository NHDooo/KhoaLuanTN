using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Models.EF;
using PagedList.Mvc;
using PagedList;
using Models.Dao;

namespace MovieProject.Areas.Admin.Controllers
{
    public class TrailerController : BaseController
    {
        // GET: Admin/Trailer
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            List<Movie> listmovi = db.Movies.ToList();
            ViewBag.listmovi = listmovi;

            var dao = new TrailerDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Trailers.SingleOrDefault(x => x.TrailerID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Add(Trailer model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.Image = model.Image.ToString();
                    model.Url = model.Url.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Trailers.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                List<Movie> listmovi = db.Movies.ToList();
                ViewBag.listmovi = listmovi;
                List<Trailer> list = GetData();
                SetAlert("Thêm Trailer thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var listupdate = db.Trailers.SingleOrDefault(x => x.TrailerID == model.TrailerID);
                    listupdate.Name = model.Name;
                    listupdate.Image = model.Image;
                    listupdate.Url = model.Url;
                    listupdate.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    listupdate.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                List<Movie> listmovi = db.Movies.ToList();
                ViewBag.listmovi = listmovi;
                List<Trailer> list = GetData();
                SetAlert("Sửa Trailer thành công", "success");
                return RedirectToAction("Index");
            }
           
            else
            {
                List<Trailer> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Trailer> GetData()
        {
            return db.Trailers.ToList();


        }
       
        public ActionResult Delete(int id)
        {
            new TrailerDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new TrailerDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }

    }
}