using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
        }

        public static class Claims
        {
            public const string Staff = "staff";

            /// <summary>
            /// Maps .NET claim types to nice short strings the SPA app can handle.
            /// </summary>
            public static readonly IReadOnlyDictionary<string, string> SpaClaimsMap = new Dictionary<string, string>()
                                                                                      {
                                                                                          {ClaimTypes.Name, "username"},
                                                                                          {ClaimTypes.NameIdentifier, "id"},
                                                                                          {ClaimTypes.Role, "role"}
                                                                                      };
        }

        public static class Subscriptions
        {
            public const string UserCaseCreated = nameof(UserCaseCreated);
            public const string OrganizationCaseCreated = nameof(OrganizationCaseCreated);
        }

        public static class Magic
        {
            /// <summary>
            /// Represents the lockout end date for a disabled account.
            /// </summary>
            public static readonly DateTimeOffset DisabledLockoutEndDate = new DateTimeOffset(3141, 5, 9, 2, 6, 5, TimeSpan.Zero);
        }

        /// <summary>
        /// Keys for ViewData properties.
        /// </summary>
        public static class ViewData
        {
            public const string ScriptList = nameof(ScriptList);
        }

        /// <summary>
        /// Names of Entity Framework filters which can be enabled or disabled.
        /// </summary>
        public static class EntityFrameworkFilters
        {
            public const string SoftDelete = nameof(SoftDelete);
        }
    }
}
