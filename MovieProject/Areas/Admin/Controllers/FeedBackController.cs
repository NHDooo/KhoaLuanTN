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
    public class FeedBackController : Controller
    {
        // GET: Admin/FeedBack
        MovieProjectDbcontext db = new MovieProjectDbcontext();

        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var dao = new FeedBackDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Feedbacks.SingleOrDefault(x => x.ID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }   
        public List<Ad> GetData()
        {
            return db.Ads.ToList();


        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new FeedBackDao().Delete(id);
            return RedirectToAction("Index");
        }
    }
}