using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Models.Admin;

namespace MichaelsPlace.Controllers
{
    /// <summary>
    /// Controller used for testing one-off things.
    /// </summary>
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult PersonReferenceModel()
        {
            var model = new PersonReferenceModel();
            return View(model);
        }
    }
}