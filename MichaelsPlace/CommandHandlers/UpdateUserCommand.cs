using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.CommandHandlers
{
    public class UpdateUserCommand : IAsyncRequest<ICommandResult>
    {
        public ModelStateDictionary ModelState { get; set; }
        public UserModel User { get; set; }
        public IEnumerable<string> RolesForPerson { get; set; }

        public UpdateUserCommand(UserModel user, IEnumerable<string> rolesForPerson, ModelStateDictionary modelState)
        {
            User = user;
            ModelState = modelState;
            RolesForPerson = rolesForPerson;
        }
    }

    public class UpdateUserCommandHandler : CommandHandlerBase, IAsyncRequestHandler<UpdateUserCommand, ICommandResult>
    {
        private Injected<ApplicationUserManager> _userManager;

        [Inject]
        public ApplicationUserManager UserManager
        {
            get { return _userManager.Value; }
            set { _userManager.Value = value; }
        }

        public async Task<ICommandResult> Handle(UpdateUserCommand message)
        {
            var id = message.User.Id;

            if (message.User.IsLockedOut == false && await UserManager.IsLockedOutAsync(id))
            {
                await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MinValue);
            }
            if (message.User.IsDisabled == true)
            {
                await UserManager.SetLockoutEndDateAsync(id, Constants.Magic.DisabledLockoutEndDate);
            }
            if (message.User.IsStaff == true)
            {
                await UserManager.EnsureHasClaimAsync(id, Constants.Claims.Staff, Boolean.TrueString);
            }
            else
            {
                await UserManager.EnsureDoesNotHaveClaimAsync(id, Constants.Claims.Staff, Boolean.TrueString);
            }

            var userRoles = await UserManager.GetRolesAsync(id);


            var result = await UserManager.AddToRolesAsync(id, message.RolesForPerson.Except(userRoles).ToArray());

            if (!result.Succeeded)
            {
                message.ModelState.AddModelError("", result.Errors.First());
                return CommandResult.Failure();
            }

            result = await UserManager.RemoveFromRolesAsync(id, userRoles.Except(message.RolesForPerson).ToArray());

            if (!result.Succeeded)
            {
                message.ModelState.AddModelError("", result.Errors.First());
                return CommandResult.Failure();

            }
            return CommandResult.Success();

        }
    }
}
