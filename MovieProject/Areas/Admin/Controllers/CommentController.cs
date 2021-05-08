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
    public class CommentController : BaseController
    {
        // GET: Admin/Comment
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchTerm)
        {
            var search = from b in db.Comments select b;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                search = db.Comments.Where(b => b.Name.Contains(searchTerm));
            }
            ViewBag.SearchTerm = searchTerm;
            List<Comment> list = GetData();
            return View(search.ToList());
        }
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Comments.SingleOrDefault(x => x.CommentID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "ADD_USER")]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Add(Comment model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.MovieID = model.MovieID;
                    model.Name = model.Name.ToString();
                    model.Content = model.Content.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Comments.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm quảng cáo thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Comments.SingleOrDefault(x => x.CommentID == model.CommentID);
                    list.MovieID = model.MovieID;
                    list.Name = model.Name;
                    list.Content = model.Content;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa quảng cáo thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<Comment> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Comment> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Comment> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Comment> GetData()
        {
            return db.Comments.ToList();


        }
        public ActionResult Delete(int id)
        {
            new CommentDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new CommentDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}