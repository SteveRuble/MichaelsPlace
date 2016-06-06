using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Models;

namespace MichaelsPlace.Controllers.Admin
{
    public class SituationController : AdminControllerBase
    {
        // GET: Situation
        public ActionResult Index()
        {
            return View();
        }
    }
}