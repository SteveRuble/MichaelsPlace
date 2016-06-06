using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;

namespace MichaelsPlace.Controllers.Admin
{

    public class UserController : AdminControllerBase
    {
        private readonly UserQueries _userQueries;
        public UserController(UserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        public ActionResult Index(bool isStaff = false)
        {
            var users = _userQueries.Execute(isStaff);
            return View(users.ToList());
        }
    }
}