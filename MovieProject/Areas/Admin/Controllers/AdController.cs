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
    [ValidateInput(false)]
    public class AdController : BaseController
    {
        // GET: Admin/Ad

        MovieProjectDbcontext db = new MovieProjectDbcontext();

        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new AdDao();
            var model = dao.ListAd(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Ads.SingleOrDefault(x => x.AdsID == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(Ad model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.NameAds = model.NameAds.ToString();
                    model.ImageAds = model.ImageAds.ToString();
                    model.UrlAds = model.UrlAds.ToString();
                    model.Status = model.Status;
                    db.Ads.Add(model);
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
                    var list = db.Ads.SingleOrDefault(x => x.AdsID == model.AdsID);
                    list.NameAds = model.NameAds;
                    list.ImageAds = model.ImageAds;
                    list.UrlAds = model.UrlAds;
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa quảng cáo thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.NameAds))
                {
                    List<Ad> list = GetData().Where(s => s.NameAds.Contains(model.NameAds)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Ad> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Ad> list = GetData().OrderBy(s => s.NameAds).ToList();
                return View("Index", list);
            }
        }
        public List<Ad> GetData()
        {
            return db.Ads.ToList();


        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new AdDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(int id)
        {
            var result = new AdDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}