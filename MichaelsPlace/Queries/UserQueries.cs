using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    public class UserQueries : QueryBase
    {
        public virtual IQueryable<ApplicationUser> Execute(bool? isStaff)
        {
            var query = DbSets.Set<ApplicationUser>().AsQueryable();
            if (isStaff == true)
            {
                query = query.Where(u => u.Claims.Any(c => c.ClaimType == Constants.Claims.Staff));
            }
            else if (isStaff == false)
            {
                query = query.Where(u => u.Claims.All(c => c.ClaimType != Constants.Claims.Staff));
            }
            return query;
        }
        
    }
}
