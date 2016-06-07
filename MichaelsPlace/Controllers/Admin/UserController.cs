using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using Microsoft.AspNet.Identity.Owin;

namespace MichaelsPlace.Controllers.Admin
{

    public class UserController : AdminControllerBase
    {
        private readonly UserQueries _userQueries;
        public UserController(UserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        public ActionResult Index()
        {
            var users = DbContext.Users.ProjectTo<UserModel>(Mapper);
            
            return View(users.ToList());
        }

        public ActionResult Edit(string userId)
        {
            var user = DbContext.Users.ProjectTo<UserModel>(Mapper).First(u => u.Id == userId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string userId, UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.First(u => u.Id == userId);

                Mapper.Map(model, user);

                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                DbContext.SaveChanges();

                if (model.IsLockedOut == false && await userManager.IsLockedOutAsync(userId))
                {
                    await userManager.SetLockoutEndDateAsync(userId, DateTimeOffset.MinValue);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }


}