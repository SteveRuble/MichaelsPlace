using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MichaelsPlace.Controllers.Api;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    /// <summary>
    /// Query which returns items which pertain to a specified situation.
    /// </summary>
    public class CasesByPersonQuery : QueryBase
    {
        public virtual IProjectableQuery<TItem> Execute<TItem>(string personId)
            where TItem : Case
        {
            var items = from item in DbSets.Set<TItem>()
                        where item.CaseUsers.Any(c => c.Person.Id == personId)
                        select item;

            return items.AsProjectable(Mapper);
        }
    }
}
