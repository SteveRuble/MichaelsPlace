using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("browsing")]
    public class BrowsingController : ApiController
    {
        private readonly IQueryFactory _queryFactory;

        public BrowsingController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        [HttpGet, Route("article/situation/{demographicId}/{lossId}/{mournerId}")]
        public List<BrowsingItemModel> ArticleBySituation(int demographicId, int lossId, int mournerId) =>
            _queryFactory.Create<ItemBySituationQuery>()
                         .Execute<Article>(demographicId, lossId, mournerId)
                         .ProjectTo<BrowsingItemModel>()
                         .ToList();

        [HttpGet, Route("todo/situation/{demographicId}/{lossId}/{mournerId}")]
        public List<BrowsingItemModel> ToDoBySituation(int demographicId, int lossId, int mournerId) =>
            _queryFactory.Create<ItemBySituationQuery>()
                         .Execute<ToDo>(demographicId, lossId, mournerId)
                         .ProjectTo<BrowsingItemModel>()
                         .ToList();
    }
}
