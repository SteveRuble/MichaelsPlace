using System.Linq;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    /// <summary>
    /// Query which returns items which pertain to a specified situation.
    /// </summary>
    public class OrganizationsByPersonQuery : QueryBase
    {
        public virtual IProjectableQuery<TItem> Execute<TItem>(string personId)
            where TItem : Organization
        {
            var items = from item in DbSets.Set<TItem>()
                        where item.OrganizationPeople.Any(o => o.Person.Id == personId)
                        select item;

            return items.AsProjectable(Mapper);
        }
    }
}
