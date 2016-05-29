using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Serilog;

namespace MichaelsPlace.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public ILogger Logger { get; set; }

        public ActionResult Index()
        {
            Logger.Information("Home");
            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Throw()
        {
            if (GlobalSettings.IsDevelopment)
            {
                throw new Exception("Test");
            }

            return HttpNotFound();
        }
    }
}