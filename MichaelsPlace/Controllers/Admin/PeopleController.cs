using System;
using System.Collections.Generic;
using System.Data.Entity;
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

    public class PeopleController : AdminControllerBase
    {
        public PeopleController()
        {
        }

        public ActionResult Index()
        {
            var people = DbContext.People.ProjectTo<PersonModel>(Mapper);
            
            return View(people.ToList());
        }

        public ActionResult Edit(string id)
        {
            var person = DbContext.People.ProjectTo<PersonModel>(Mapper).First(u => u.Id == id);

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, PersonModel model)
        {
            if (ModelState.IsValid)
            {
                var person = DbContext.People.Include(p => p.ApplicationUser).First(u => u.Id == id);

                Mapper.Map(model, person);

                DbContext.SaveChanges();

                if (person.ApplicationUser != null)
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    if (model.IsLockedOut == false && await userManager.IsLockedOutAsync(id))
                    {
                        await userManager.SetLockoutEndDateAsync(id, DateTimeOffset.MinValue);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }


}