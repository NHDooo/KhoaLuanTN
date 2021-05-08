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
    [ValidateInput(false)]
    public class NewController : BaseController
    {

        // GET: Admin/New
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new NewDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.News.SingleOrDefault(x => x.NewsID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]

        public ActionResult Add(News model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.Image = model.Image.ToString();
                    model.Description = model.Description.ToString();
                    model.Year = model.Year;
                    model.Viewed = model.Viewed;
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    model.ImageNews = model.ImageNews;
                    model.MoreDescription = model.MoreDescription.ToString();
                    db.News.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm tin tức thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.News.SingleOrDefault(x => x.NewsID == model.NewsID);
                    list.Name = model.Name;
                    list.Image = model.Image;
                    list.Description = model.Description;
                    list.Year = model.Year;
                    list.Viewed = model.Viewed;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);              
                    list.Status = model.Status;
                    list.ImageNews = model.ImageNews;
                    list.MoreDescription = model.MoreDescription;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa tin tức thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<News> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<News> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<News> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<News> GetData()
        {
            return db.News.ToList();


        }
        public ActionResult Delete(int id)
        {
            new NewDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new NewDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }


    }
}