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
    public class SlideController : BaseController
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        // GET: Admin/Slide
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new SlideDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Slides.SingleOrDefault(x => x.SlideID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]

        public ActionResult Add(Slide model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.Description = model.Description.ToString();
                    model.Image = model.Image.ToString();
                    model.Url = model.Url.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;

                    db.Slides.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm Slide thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Slides.SingleOrDefault(x => x.SlideID == model.SlideID);
                    list.Name = model.Name;
                    list.Description = model.Description;
                    list.Image = model.Image;
                    list.Url = model.Url;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa Slide thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<Slide> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Slide> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Slide> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Slide> GetData()
        {
            return db.Slides.ToList();


        }
        public ActionResult Delete(int id)
        {
            new SlideDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new SlideDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }


    }
}