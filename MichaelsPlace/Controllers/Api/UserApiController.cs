using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MichaelsPlace.Extensions;

namespace MichaelsPlace.Controllers.Api
{
    public class UserClaimModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    [RoutePrefix("user")]
    public class UserApiController : SpaApiControllerBase
    {

        /// <summary>
        /// Returns a dictionary of the claim values available to the SPA.
        /// This claims are for use in building the correct UI, not for enforcing security.
        /// </summary>
        /// <returns></returns>
        [Route("claims")]
        [HttpGet]
        public List<UserClaimModel> Claims() => User.GetClaimsIdentityOrAnonymous().Claims
            .Where(c => Constants.Claims.SpaClaimsMap.ContainsKey(c.Type))
            .Select(c => new UserClaimModel() {Type = Constants.Claims.SpaClaimsMap[c.Type], Value = c.Value}).ToList();
    }
}
