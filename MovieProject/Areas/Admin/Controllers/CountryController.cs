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
    public class CountryController : BaseController
    {
        // GET: Admin/Country
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new CountryDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Countries.SingleOrDefault(x => x.CountryID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(Country model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Countries.Add(model);
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
                    var list = db.Countries.SingleOrDefault(x => x.CountryID == model.CountryID);
                    list.Name = model.Name;
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
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<Country> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Country> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Country> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Country> GetData()
        {
            return db.Countries.ToList();


        }
        public ActionResult Delete(int id)
        {
            new CountryDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new CountryDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}