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
    public class ItemBySituationQuery : QueryBase
    {
        public virtual IProjectableQuery<TItem> Execute<TItem>(SituationModel situationModel)
            where TItem : Item
        {
            var items = from item in DbSets.Set<TItem>()
                        where item.AppliesToContexts.Any(c => situationModel.Contexts.Contains(c.Id))
                              && item.AppliesToLosses.Any(c => situationModel.Losses.Contains(c.Id))
                              && item.AppliesToRelationships.Any(c => situationModel.Relationships.Contains(c.Id))
                        select item;

            return items.AsProjectable(Mapper);
        }
    }
}
