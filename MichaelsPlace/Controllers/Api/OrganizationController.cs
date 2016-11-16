using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Api.OrganizationDashboard;
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

        /// <summary>
        /// Gets organization and related information from an organizationId.
        /// </summary>
        /// <param name="organizationId">The id of the organization</param>
        /// <returns>An object with all the details about the organization</returns>
        [HttpGet, Route("getOrganization/{organizationId:int}")]
        public OrganizationViewModel GetOrganization(int organizationId)
        {
            return _queryFactory.Create<OrganizationByIdQuery>()
                        .Execute<Organization>(organizationId)
                        .ProjectTo<OrganizationViewModel>()
                        .FirstOrDefault();
        }

        /// <summary>
        /// Creates an organization.
        /// </summary>
        /// <param name="payload">The HTTP Post Payload, containing all the details necessary to create an organization.</param>
        /// <returns>The organizationId of the newly created organization</returns>
        [HttpPost, Route("create")]
        public async Task<string> CreateOrganization([FromBody] NewOrganizationModel payload)
        {
            var request = new CreateOrganizationCommand.Request()
            {
                Name = payload.Name,
                PhoneNumber = payload.PhoneNumber,
                FaxNumber = payload.FaxNumber,
                Notes = payload.Notes,
                Address = new Address()
                {
                    LineOne = payload.Line1,
                    LineTwo = payload.Line2,
                    City = payload.City,
                    State = payload.State,
                    Zip = payload.Zip
                },
                UserId = User.Identity.GetUserId()
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as string;
        }
    }
}