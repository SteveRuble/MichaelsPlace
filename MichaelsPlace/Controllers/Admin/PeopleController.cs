using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Extensions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{
    public enum PeopleIndexDisplayMode
    {
        All,
        Staff,
        Clients,
    }

    public class PeopleIndexViewModel
    {
        public PeopleIndexDisplayMode DisplayMode { get; set; }
        public List<PersonModel> People { get; set; }
    }

    public class PersonEditViewModel
    {
        public PersonModel Person { get; set; }
    }

    public class PeopleController : AdminControllerBase
    {
        private Injected<ApplicationUserManager> _userManager;

        [Inject]
        public ApplicationUserManager UserManager
        {
            get { return _userManager.Value; }
            set { _userManager.Value = value; }
        }

        private Injected<ApplicationRoleManager> _roleManager;

        [Inject]
        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager.Value; }
            set { _roleManager.Value = value; }
        }


        public PeopleController()
        {
        }

        public ActionResult Index(PeopleIndexDisplayMode displayMode = PeopleIndexDisplayMode.All)
        {
            var people = GetPersonModels(displayMode);

            var model = new PeopleIndexViewModel()
                        {

                            DisplayMode = displayMode,
                            People = people.ToList(),
                        };

            return View(model);
        }
        
        public JsonResult JsonIndex([ModelBinder(typeof(DataTables.AspNet.Mvc5.ModelBinder))] IDataTablesRequest requestModel, PeopleIndexDisplayMode displayMode = PeopleIndexDisplayMode.All)
        {
            var models = GetPersonModels(displayMode);

            var response = requestModel.ApplyTo(models);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }
        
        private IQueryable<PersonModel> GetPersonModels(PeopleIndexDisplayMode displayMode)
        {
            var query = DbContext.People.AsQueryable();
            switch (displayMode)
            {
                case PeopleIndexDisplayMode.All:
                    break;
                case PeopleIndexDisplayMode.Staff:
                    query = query.Where(p => p.ApplicationUser.Claims.Any(c => c.ClaimType == Constants.Claims.Staff));
                    break;
                case PeopleIndexDisplayMode.Clients:
                    query = query.Where(p => p.ApplicationUser.Claims.All(c => c.ClaimType != Constants.Claims.Staff));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(displayMode), displayMode, null);
            }

            var people = query.ProjectTo<PersonModel>(Mapper);
            return people;
        }

        public ActionResult Details(string id)
        {
            var person = DbContext.People.ProjectTo<PersonModel>(Mapper).First(u => u.Id == id);

            var viewModel = new PersonEditViewModel()
                            {
                                Person = person,
                            };

            return PartialView(viewModel);
        }

        public ActionResult Edit(string id)
        {
            var person = DbContext.People.ProjectTo<PersonModel>(Mapper).First(u => u.Id == id);

            var viewModel = new PersonEditViewModel()
                            {
                                Person = person,
                            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, PersonEditViewModel model, List<string> selectedRoles)
        {
            selectedRoles = selectedRoles ?? new List<string>();

            if (ModelState.IsValid)
            {
                var command = new AddOrEditPersonCommand(model.Person, ModelState);

                var commandResult = await Mediator.SendAsync(command);
                if (commandResult.IsSuccess)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                }
            }

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return null;
        }
    }
}