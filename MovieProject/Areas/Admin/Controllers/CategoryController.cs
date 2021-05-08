using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Models.EF;
using PagedList.Mvc;
using PagedList;
using Models.Dao;
using System.Web.ModelBinding;


namespace MovieProject.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new CategoryDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Categories.SingleOrDefault(x => x.CategoryID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(Category model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.NameCategory = model.NameCategory.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Categories.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm thể loại thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Categories.SingleOrDefault(x => x.CategoryID == model.CategoryID);
                    list.NameCategory = model.NameCategory;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa thể loại thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.NameCategory))
                {
                    List<Category> list = GetData().Where(s => s.NameCategory.Contains(model.NameCategory)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Category> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Category> list = GetData().OrderBy(s => s.NameCategory).ToList();
                return View("Index", list);
            }
        }
        public List<Category> GetData()
        {
            return db.Categories.ToList();


        }
        public ActionResult Delete(int id)
        {
            new CategoryDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new CategoryDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}