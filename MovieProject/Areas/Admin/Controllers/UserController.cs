using Models.Dao;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Security.Cryptography;
using Newtonsoft.Json;
using MovieProject.Common;

namespace MovieProject.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID ="VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            List<UserGroup> listper = db.UserGroups.ToList();
            ViewBag.listper = listper;

            var dao = new UserDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Users.SingleOrDefault(x => x.UserID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Add(User model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.UserName = model.UserName.ToString();
                    model.Password = Encryptor.MD5Hash(model.Password.ToString());
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    model.GroupID = model.GroupID.ToString();
                    db.Users.Add(model);
                    db.SaveChanges();
                    model = null;
                   
                }
                List<UserGroup> listper = db.UserGroups.ToList();
                ViewBag.listuser = listper;
                List<User> list = GetData();
                ViewBag.list = list;
                SetAlert("Thêm user thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var listupdate = db.Users.SingleOrDefault(x => x.UserID == model.UserID);
                    listupdate.Name = model.Name;
                    listupdate.UserName = model.UserName;
                    listupdate.Password = Encryptor.MD5Hash(model.Password);
                    listupdate.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    listupdate.Status = model.Status;
                    model.GroupID = model.GroupID;
                    db.SaveChanges();
                    model = null;
                }
                List<UserGroup> listper = db.UserGroups.ToList();
                ViewBag.listuser = listper;
                List<User> list = GetData();
                ViewBag.list = list;
                SetAlert("Sửa user thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    List<User> list = GetData().Where(s => s.UserName.Contains(model.UserName)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<User> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<User> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<User> GetData()
        {
            return db.Users.ToList();


        }
       
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }
        [HttpDelete]
       
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return Redirect("Index");
        }
        public ActionResult LogoutAD ()
        {
            Session[Common.CommonContants.USER_SESSION] = null;
            return Redirect("/Admin/Login");
        }

    }
}