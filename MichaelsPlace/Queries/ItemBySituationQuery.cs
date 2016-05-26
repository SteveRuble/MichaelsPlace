using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MichaelsPlace.Controllers.Api;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    /// <summary>
    /// Query which returns items which pertain to a specified situation.
    /// </summary>
    public class ItemBySituationQuery : IQuery
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ItemBySituationQuery(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public virtual IProjectableQuery<TItem> Execute<TItem>(int demographicId, int lossId, int mournerId)
            where TItem : Item
        {
            var items = from item in _dbContext.Items.OfType<TItem>()
                        from situation in item.Situations
                        where situation.Losses.Any(x => x.Id == lossId)
                              && situation.Mourners.Any(x => x.Id == mournerId)
                              && situation.Demographics.Any(x => x.Id == demographicId)
                        select item;

            return items.AsProjectable(_mapper);
        }
    }
}
