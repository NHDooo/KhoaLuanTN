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
using System.Web.Hosting;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace MovieProject.Controllers
{
    public class UserTController : Controller
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: UserT
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.UserID;
                    Session.Add(CommonContants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }
        public ActionResult Logout()
        {
            Session[Common.CommonContants.USER_SESSION] = null;
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.UserID;
                    Session.Add(Common.CommonContants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }

                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công!");
                }
            }
            return View(model);
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCapcha", "Xác nhận mã không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
               
            else
            {
                    
                var user = new User();
                user.Name = model.Name;
                user.UserName = model.UserName;
                user.Password = Encryptor.MD5Hash(model.Password);
                user.GroupID = "MEMBER";
                user.CreatedDate = DateTime.Now;
                user.Status = true;
                user.Phone = model.Phone;
                user.Email = model.Email;
                var result = dao.InsertUser(user);
                BuildEmailTemplate(user.UserID);
                if (result > 0)
                {
                    ViewBag.Success = "Đăng ký thành công hãy kiểm tra email của bạn để hoàn tất quá trình đăng nhập!";
                    model = new RegisterModel();
                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký không thành công");
                }


            }
            }
            return View(model);
        }

        public void BuildEmailTemplate(int RegID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var RegInfo = db.Users.Where(x => x.UserID == RegID).FirstOrDefault();
            var url = "http://localhost:52046/" + "UserT/Confirm?RegID=" + RegID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your account is successfully created", body, RegInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "onemoviesct@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);


        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("onemoviesct@gmail.com", "nguyenhuudo");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);


        }

        public ActionResult Confirm(int RegID)
        {
            ViewBag.RegID = RegID;
            return View();
        }
        public JsonResult RegisterConfirm(int RegID)
        {
            //User Data = db.Users.Where(x => x.UserID == RegID).FirstOrDefault();
            //db.SaveChanges();
            var msg = "Your email is verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult ToolsTop()
        {

            return PartialView();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.UserName = model.UserName;
                user.Password = Encryptor.MD5Hash(model.NewPassword);
                

            }
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }
       
        public void SendVerificationEmail(string emailID, string acticationCode)
        {
            
            var verifyUrl =  "/UserT/ResetPassword/" + acticationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("onemoviesct@gmail.com", "OneMovies xem phim trực tuyến.");
            var toEmail = new MailAddress(emailID);
            var fromEmailPass = "nguyenhuudo";
            string subject = "Quên mật khẩu";

            string body = "Xin chào <br/><br/>Chúng tôi vừa nhận được yêu cầu đổi mật khẩu, nếu đó là bạn hãy nhấn vào đường link bên dưới." +
                "<br/><br/><a href=" + link + ">Đổi mật khẩu</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPass)
            };
            MailMessage mail = new MailMessage(fromEmail,toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            smtp.Send(mail);

            

        }

        /// Forget Password
        public ActionResult ForgotPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            bool status = false;
            var account = db.Users.Where(x => x.Email == EmailID).FirstOrDefault();
            if(account != null)
            {
                // Gui den email da nhap
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationEmail(account.Email, resetCode);
                account.ResetPasswordCode = resetCode;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Đã gửi yêu cầu đổi mật khẩu đến Email vừa nhập";
            }
            else
            {
                message = "Tài khoản không tồn tại";
            }


            return View();
        }
        public ActionResult ResetPassword(string id)
        {
            // xac nhan email
            // tim duong dan
            var user = db.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if(user != null)
                {
                    user.Password = Encryptor.MD5Hash(model.NewPassword);
                    user.ResetPasswordCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "Đổi mật khẩu thành công!";
                }
            }
            else
            {
                message = "Something Invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
}