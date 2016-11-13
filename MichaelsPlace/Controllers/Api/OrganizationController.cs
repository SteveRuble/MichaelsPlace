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
    [RoutePrefix("organization")]
    public class OrganizationController : SpaApiControllerBase
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IMediator _mediator;

        public OrganizationController(IQueryFactory queryFactory, IMediator mediator)
        {
            _queryFactory = queryFactory;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all organizations for the logged in user.
        /// </summary>
        /// <returns>A list of organizationIds and their titles</returns>
        [HttpGet, Route("getOrganizations")]
        public List<OrganizationListModel> OrganizationsByPerson()
        {
            string userId = User.Identity.GetUserId();

            return _queryFactory.Create<OrganizationsByPersonQuery>()
                         .Execute<Organization>(userId)
                         .ProjectTo<OrganizationListModel>()
                         .ToList();
        }
    }
}