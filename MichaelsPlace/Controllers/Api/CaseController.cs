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
        /// Gets all cases for the logged in user.
        /// </summary>
        /// <returns>A list of caseIds and their titles</returns>
        [HttpGet, Route("getCases")]
        public List<CaseListModel> CasesByPerson()
        {
            string userId = User.Identity.GetUserId();

            return _queryFactory.Create<CasesByPersonQuery>()
                         .Execute<Case>(userId)
                         .ProjectTo<CaseListModel>()
                         .ToList();
        }

        /// <summary>
        /// Creates a case based on the situation.
        /// </summary>
        /// <param name="situation">The situation the user navigated to</param>
        /// <param name="title">The title of the user</param>
        /// <returns>The caseId of the newly created case</returns>
        [HttpGet, Route("create/{situation:situation}/{title}")]
        public async Task<string> CreateCaseBySituation(SituationModel situation, string title)
        {
            var request = new CreateCaseCommand.Request()
            {
                Situation = situation,
                Title = title,
                UserId = User.Identity.GetUserId()
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        }
    }
}