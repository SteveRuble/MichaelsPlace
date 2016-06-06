using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MichaelsPlace.Extensions
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Returns the user's GUID.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetUserId([NotNull] this IPrincipal principal)
        {
            var claimsIdentity = GetClaimsIdentity(principal);
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static ClaimsIdentity GetClaimsIdentity(this IPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            var claimsIdentity = (ClaimsIdentity) principal.Identity;
            return claimsIdentity;
        }
    }
}
