using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Queries
{
    public class AdminTagModelQuery : QueryBase
    {
        public List<AdminTagModel> GetAdminTagModels()
        {
            var tags = this.DbSets.Set<Tag>().ToList();

            var models =
                tags.OfType<ContextTag>()
                    .Select(Mapper.Map<AdminTagModel>)
                    .Concat(tags.OfType<LossTag>().Select(Mapper.Map<AdminTagModel>))
                    .Concat(tags.OfType<RelationshipTag>().Select(Mapper.Map<AdminTagModel>))
                    .ToList();

            return models;
        }
    }
}
