using Models.Dao;
using Models.EF;
using MovieProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieProject.Common;
using BotDetect.Web.Mvc;
using Facebook;
using System.Configuration;
using Common;
using WebMatrix.WebData;


namespace MovieProject.Controllers
{
    public class ChangePasswordController : Controller
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        // GET: ForgetPass
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet, Authorize]
        public ActionResult ChangePass()
        {
           
            return View();
        }
        [HttpGet, Authorize, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel passModel)
        {
            if (ModelState.IsValid)
            {
                bool isPasswordChange = WebSecurity.ChangePassword(WebSecurity.CurrentUserName, passModel.OldPasswword, passModel.NewPassword);

                if (isPasswordChange)
                {
                    RedirectToAction("Login", "UserT");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại khồn đúng");
                }
            }
            return View();
        }

    }
}