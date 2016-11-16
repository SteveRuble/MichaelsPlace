using System.Linq;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    public class OrganizationByIdQuery : QueryBase
    {
        /// <summary>
        /// Query which returns a full organization for displaying on the organization dashboard.
        /// </summary>
        public virtual IProjectableQuery<Organization> Execute<TItem>(int organizationId)
        {
            var organization = from organizations in DbSets.Set<Organization>()
                               where organizations.Id == organizationId
                               select organizations;

            return organization.AsProjectable(Mapper);
        }
    }
}