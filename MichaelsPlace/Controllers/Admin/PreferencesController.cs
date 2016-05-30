using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MichaelsPlace.Controllers.Admin
{
    public class PreferencesController : AdminControllerBase
    {
        // GET: Preferences
        public ActionResult StaffAlerts()
        {
            return View();
        }
    }
}