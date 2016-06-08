using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity.Owin;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{

    public class PeopleController : AdminControllerBase
    {
        private Injected<ApplicationUserManager> _userManager;

        [Inject]
        public ApplicationUserManager UserManager
        {
            get { return _userManager.Value; }
            set { _userManager.Value = value; }
        }

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
                    if (model.IsLockedOut == false && await UserManager.IsLockedOutAsync(id))
                    {
                        var result = await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MinValue);
                    }
                    if (model.IsDisabled)
                    {
                        var result = await UserManager.SetLockoutEndDateAsync(id, Constants.Magic.DisabledLockoutEndDate);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }


}