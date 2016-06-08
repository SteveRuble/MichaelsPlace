using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
