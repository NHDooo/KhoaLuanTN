using Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    public class AboutMController : Controller
    {
        // GET: AboutM
        public ActionResult Index()
        {
            var model = new AboutDao().GetContentAbout();
            return View(model);
        }
    }
}