using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("case")]
    public class CaseController : SpaApiControllerBase
    {
        private readonly IQueryFactory _queryFactory;

        public CaseController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        /// <summary>
        /// Returns all cases for the person with <paramref name="personId"/>
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("cases/{personId}")]
        public List<Case> CasesByPerson(string personId) =>
            _queryFactory.Create<CasesByPersonQuery>()
                         .Execute<Case>(personId)
                         .ToList();
    }
}