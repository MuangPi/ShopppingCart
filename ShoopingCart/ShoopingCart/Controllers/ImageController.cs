using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoopingCart.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BindData()
        {
            var image = Server.MapPath("~/App_Data/com.jpg");
            return base.File(image, "image/jpg");
            
        }
    }
}