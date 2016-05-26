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

        [HttpGet, Route("demographic")]
        public IEnumerable<Tag> TeamMember() =>_dbContext.Tags.OfType<DemographicTag>().ToList();

        [HttpGet, Route("loss")]
        public IEnumerable<Tag> Loss() =>_dbContext.Tags.OfType<LossTag>();

        [HttpGet, Route("mourner")]
        public IEnumerable<Tag> Mourner() =>_dbContext.Tags.OfType<MournerTag>();
    }
}