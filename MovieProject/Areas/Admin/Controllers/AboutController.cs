using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Models.EF;
using PagedList;
using Models.Dao;

namespace MovieProject.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class AboutController : BaseController
    {
        // GET: Admin/About
        MovieProjectDbcontext db = new MovieProjectDbcontext();

        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new AboutDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Abouts.SingleOrDefault(x => x.AboutID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(About model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.Image = model.Image.ToString();
                    model.Description = model.Description.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Abouts.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm giới thiệu thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Abouts.SingleOrDefault(x => x.AboutID == model.AboutID);
                    list.Name = model.Name;
                    list.Image = model.Image;
                    list.Description = model.Description;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa giới thiệu thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<About> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<About> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<About> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<About> GetData()
        {
            return db.Abouts.ToList();


        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new AboutDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new AboutDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}