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
            var items = from item in DbContext.Items.OfType<TItem>()
                        from situation in item.Situations
                        where situation.Losses.Any(x => situationModel.Losses.Contains(x.Id))
                              && situation.Mourners.Any(x => situationModel.Mourners.Contains(x.Id))
                              && situation.Demographics.Any(x => situationModel.Demographics.Contains(x.Id))
                        select item;

            return items.AsProjectable(Mapper);
        }
    }
}
