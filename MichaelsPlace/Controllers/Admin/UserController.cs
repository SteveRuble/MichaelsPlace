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
    public class UserEditViewModel
    {
        public PersonModel Person { get; set; }

        public UserModel User { get; set; }

        public List<SelectListItem> RolesList { get; set; } 
    }

    public class UserController : AdminControllerBase
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
        
        public ActionResult Edit(string id)
        {
            var person = DbContext.People.ProjectTo<PersonModel>(Mapper).FirstOrDefault(u => u.Id == id);
            var user = DbContext.Users.ProjectTo<UserModel>(Mapper).FirstOrDefault(u => u.Id == id);

            if (person == null || user == null)
            {
                return HttpNotFound();
            }

            var viewModel = new UserEditViewModel()
                            {
                                Person = person,
                                User = user,
                                RolesList = RoleManager.Roles.ToList()
                                                   .Select(r => new SelectListItem()
                                                                {
                                                                    Selected = user.Roles.Contains(r.Id),
                                                                    Text = r.Name,
                                                                    Value = r.Id
                                                                })
                                                   .ToList()
                            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserEditViewModel model, List<string> selectedRoles)
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
                var command = new UpdateUserCommand(model.User, selectedRoles, ModelState);

                var result = await Mediator.SendAsync(command);

                if (result.IsSuccess)
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