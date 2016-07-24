using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("tags")]
    public class TagsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public TagsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext.ConfiguredForFastQueries();
        }

        [HttpGet, Route("relationship")]
        public IEnumerable<RelationshipTag> Relationship(int? contextId = null) =>
            _dbContext.Tags.OfType<RelationshipTag>().Where(r => (contextId == null || contextId == r.Context.Id)).ToList();

        [HttpGet, Route("loss")]
        public IEnumerable<LossTag> Loss(int? contextId = null) =>
            _dbContext.Tags.OfType<LossTag>().Where(t => contextId == null || contextId == t.Context.Id).ToList();

        [HttpGet, Route("context")]
        public IEnumerable<ContextTag> Context() =>_dbContext.Tags.OfType<ContextTag>().ToList();
        
    }
}