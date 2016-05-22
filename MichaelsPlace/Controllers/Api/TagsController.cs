using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Controllers.Api
{
    public class TagsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;

        public TagsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext.ConfiguredForFastQueries();
        }

        [HttpGet, Route("tags/demographic")]
        public IEnumerable<Tag> TeamMember() =>_dbContext.Tags.OfType<DemographicTag>().ToList();

        [HttpGet, Route("tags/loss")]
        public IEnumerable<Tag> Loss() =>_dbContext.Tags.OfType<LossTag>();

        [HttpGet, Route("tags/mourner")]
        public IEnumerable<Tag> Mourner() =>_dbContext.Tags.OfType<MournerTag>();
    }
}