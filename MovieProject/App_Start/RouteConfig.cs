using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}",
     new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Category",
                url: "theloai/{idcate}",
                defaults: new { controller = "M", action = "CategoryPage", id = UrlParameter.Optional },
                namespaces: new[] { "MovieProject.Controllers" }
            );
            routes.MapRoute(
               name: "quoc gia",
               url: "country/{idc}",
               defaults: new { controller = "M", action = "CountryPage", id = UrlParameter.Optional },
               namespaces: new[] { "MovieProject.Controllers" }
           );
            routes.MapRoute(
               name: "Movie Detail",
               url: "phim/{id}",
               defaults: new { controller = "M", action = "MovieDetail", id = UrlParameter.Optional },
               namespaces: new[] { "MovieProject.Controllers" }
           );
            routes.MapRoute(
               name: "New Detail",
               url: "tintuc/{id}",
               defaults: new { controller = "NewM", action = "NewDetail", id = UrlParameter.Optional },
               namespaces: new[] { "MovieProject.Controllers" }
           );
            routes.MapRoute(
                name: "Tin Tuc",
                url: "new",
                defaults: new { controller = "NewM", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MovieProject.Controllers" }
            );
            routes.MapRoute(
               name: "Gioi thieu",
               url: "about",
               defaults: new { controller = "AboutM", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "MovieProject.Controllers" }
           );
            routes.MapRoute(
               name: "Tim kiem",
               url: "search",
               defaults: new { controller = "Home", action = "Search", id = UrlParameter.Optional },
               namespaces: new[] { "MovieProject.Controllers" }
           );
            routes.MapRoute(
              name: "Lien he",
              url: "contact",
              defaults: new { controller = "ContactM", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "MovieProject.Controllers" }
          );

            routes.MapRoute(
             name: "Error page",
             url: "error",
             defaults: new { controller = "ERROR", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "MovieProject.Controllers" }
         );

            routes.MapRoute(
           name: "dang ky",
           url: "dangky",
           defaults: new { controller = "UserT", action = "Register", id = UrlParameter.Optional },
           namespaces: new[] { "MovieProject.Controllers" }
       );
            routes.MapRoute(
          name: "dang nhap",
          url: "dangnhap",
          defaults: new { controller = "UserT", action = "Login", id = UrlParameter.Optional },
          namespaces: new[] { "MovieProject.Controllers" }
      );
            routes.MapRoute(
       name: "doi mat khau",
       url: "ChangePassword",
       defaults: new { controller = "UserT", action = "ChangePassword", id = UrlParameter.Optional },
       namespaces: new[] { "MovieProject.Controllers" }
       );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MovieProject.Controllers" }
            );

        }

    }
}
