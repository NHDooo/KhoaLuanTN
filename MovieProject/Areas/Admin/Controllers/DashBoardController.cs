using Models.Dao;
using Models.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieProject.Common;
namespace MovieProject.Areas.Admin.Controllers
{
    public class DashBoardController : BaseController
    {
        
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Movies.SingleOrDefault(x => x.MovieID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET: Admin/DashBoard
        public ActionResult Index(int page = 1,int pageSize =10)
        {
            var dao = new MovieDao();
            var model = dao.ListM(page, pageSize);
            return View(model);
        }
        [HttpPost]
        public ActionResult Dashboard()
        {
            
            return View();
        }
    }
}