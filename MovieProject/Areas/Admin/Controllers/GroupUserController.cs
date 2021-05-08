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
    public class GroupUserController : BaseController
    {
        // GET: Admin/GroupUser
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {


            var dao = new GroupUserDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(UserGroup model, String submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.ID = model.ID.ToString();
                    model.Name = model.Name.ToString();
                }
                SetAlert("Thêm quyền thành công", "success");
                return RedirectToAction("Index");
            }
            else if(submit == "Cập nhật")
            {
                if(model != null)
                {
                    var list = db.UserGroups.SingleOrDefault(x => x.ID == model.ID);
                    list.ID = model.ID;
                    list.Name = model.Name;
                    db.SaveChanges();
                    model = null; 
                }
                SetAlert("Sửa quyền thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
             
                return View("Index");
            }
        }
        public List<UserGroup> GetData()
        {
            return db.UserGroups.ToList();


        }

    }
}