using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataTables.Mvc;
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
        public List<SelectListItem> RolesList { get; set; } 
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
        
        public JsonResult JsonIndex([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, PeopleIndexDisplayMode displayMode = PeopleIndexDisplayMode.All)
        {
            var models = GetPersonModels(displayMode).OrderBy(m => m.FirstName);

            var searchString = requestModel.Search.Value;
            var filtered = searchString == null
                ? models
                : models.Where(m => m.FirstName.Contains(searchString)
                                    || m.LastName.Contains(searchString)
                                    || m.EmailAddress.Contains(searchString));

            
            var paged = filtered.Skip(requestModel.Start).Take(requestModel.Length);

            return Json(new DataTablesResponse(requestModel.Draw, paged, models.Count(), models.Count()), JsonRequestBehavior.AllowGet);
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

        public ActionResult Edit(string id)
        {
            var person = DbContext.People.ProjectTo<PersonModel>(Mapper).First(u => u.Id == id);

            var viewModel = new PersonEditViewModel()
                            {
                                Person = person,
                                RolesList = RoleManager.Roles.ToList()
                                                   .Select(r => new SelectListItem()
                                                                {
                                                                    Selected = person.Roles.Contains(r.Id),
                                                                    Text = r.Name,
                                                                    Value = r.Id
                                                                })
                                                   .ToList()
                            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, PersonEditViewModel model, List<string> selectedRoles)
        {
            selectedRoles = selectedRoles ?? new List<string>();
            model.RolesList = RoleManager.Roles.ToList()
                                            .Select(r => new SelectListItem()
                                                        {
                                                            Selected = selectedRoles.Contains(r.Id),
                                                            Text = r.Name,
                                                            Value = r.Id
                                                        })
                                            .ToList();
            if (ModelState.IsValid)
            {
                var person = DbContext.People.Include(p => p.ApplicationUser).First(u => u.Id == id);

                Mapper.Map(model.Person, person);

                DbContext.SaveChanges();

                if (person.ApplicationUser != null)
                {
                    if (model.Person.IsLockedOut == false && await UserManager.IsLockedOutAsync(id))
                    {
                        await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MinValue);
                    }
                    if (model.Person.IsDisabled)
                    {
                        await UserManager.SetLockoutEndDateAsync(id, Constants.Magic.DisabledLockoutEndDate);
                    }
                    if (model.Person.IsStaff)
                    {
                        await UserManager.EnsureHasClaimAsync(id, Constants.Claims.Staff, Boolean.TrueString);
                    }
                    else
                    {
                        await UserManager.EnsureDoesNotHaveClaimAsync(id, Constants.Claims.Staff, Boolean.TrueString);
                    }

                    var userRoles = await UserManager.GetRolesAsync(id);


                    var result = await UserManager.AddToRolesAsync(id, selectedRoles.Except(userRoles).ToArray<string>());

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return PartialView();
                    }
                    result = await UserManager.RemoveFromRolesAsync(id, userRoles.Except(selectedRoles).ToArray<string>());

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return PartialView();
                    }
                }

                return PartialView("EditCompleted");
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