using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Api.CaseDashboard;
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
        /// Gets case and related information from a caseId.
        /// </summary>
        /// <param name="caseId">The id of the case</param>
        /// <returns>An object with all the details about the case</returns>
        [HttpGet, Route("getCase/{caseId}")]
        public CaseViewModel GetCase(string caseId)
        {
            return _queryFactory.Create<CaseByIdQuery>()
                        .Execute<Case>(caseId)
                        .ProjectTo<CaseViewModel>()
                        .FirstOrDefault();
        }

        /// <summary>
        /// Creates a case based on the situation.
        /// </summary>
        /// <param name="payload">The HTTP Post Payload, containing the situation and the case title.</param>
        /// <returns>The caseId of the newly created case</returns>
        [HttpPost, Route("create")]
        public async Task<string> CreateCaseBySituation([FromBody] NewCaseModel payload)
        {
            var request = new CreateCaseCommand.Request()
            {
                Situation = payload.Situation,
                Title = payload.Title,
                UserId = User.Identity.GetUserId()
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        }

        [HttpPost, Route("createOrganizationCase")]
        public async Task<string> CreateOrganizationCaseBySituation([FromBody] NewOrganizationCaseModel payload)
        {
            var request = new CreateOrganizationCaseCommand.Request()
            {
                Situation = payload.Situation,
                Title = payload.Title,
                OrganizationId = payload.OrganizationId
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        }

        [HttpPost, Route("delete")]
        public async Task<string> DeleteCase([FromBody] string caseId)
        {
            var request = new DeleteCaseCommand.Request()
            {
                CaseId = caseId
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        } 

    }
}