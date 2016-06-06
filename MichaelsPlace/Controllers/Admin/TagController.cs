using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MichaelsPlace.Controllers.Admin
{
    public class TagController : AdminControllerBase
    {
        // GET: Tag
        public ActionResult Index()
        {
            return View();
        }
    }
}