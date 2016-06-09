using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MichaelsPlace.Controllers
{
    public class PartialsController : Controller
    {
        public ActionResult Render(string path)
        {
            return PartialView($"~/Views/{path.Replace(".", "/")}.cshtml");
        }
    }
}
