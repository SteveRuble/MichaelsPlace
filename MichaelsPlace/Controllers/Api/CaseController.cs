using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("case")]
    public class CaseController : SpaApiControllerBase
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IMediator _mediator;

        public CaseController(IQueryFactory queryFactory, IMediator mediator)
        {
            _queryFactory = queryFactory;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns all cases for the person with <paramref name="personId"/>
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("cases/{personId}")] // todo : Remove person id and pass it in through the backend
        public List<CaseModel> CasesByPerson(string personId) =>
            _queryFactory.Create<CasesByPersonQuery>()
                         .Execute<Case>(personId)
                         .ProjectTo<CaseModel>()
                         .ToList();

        [HttpPost, Route("create/{situation:situation}")]
        public async Task<string> CreateCaseBySituation(SituationModel situation)
        {
            var request = new CreateCaseCommand.Request()
            {
                Situation = situation,
                UserId = User.Identity.GetUserId()
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        }
    }
}