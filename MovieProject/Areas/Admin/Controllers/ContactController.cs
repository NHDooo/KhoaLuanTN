using Models.Dao;
using Models.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new ContactDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Contacts.SingleOrDefault(x => x.ID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Add(Contact model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Content = model.Content.ToString();
                    model.Status = model.Status;
                    db.Contacts.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm liên hệ thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Contacts.SingleOrDefault(x => x.Content == model.Content);
                    list.Content = model.Content;
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa liên hệ thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Content))
                {
                    List<Contact> list = GetData().Where(s => s.Content.Contains(model.Content)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Contact> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Contact> list = GetData().OrderBy(s => s.Content).ToList();
                return View("Index", list);
            }
        }
        public List<Contact> GetData()
        {
            return db.Contacts.ToList();


        }
        public ActionResult Delete(int id)
        {
            new ContactDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new ContactDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }


    }
}