using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Controllers.Admin
{
    public class DashboardController : AdminControllerBase
    {
        public ActionResult Index()
        {
            return View(DbContext);
        }
    }
}