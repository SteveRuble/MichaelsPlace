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
using MichaelsPlace.Services.Messaging;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("email")]
    public class EmailController : SpaApiControllerBase
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IMediator _mediator;

        private readonly string StaffAddress = "MichaelsPlace@mymichaelsplace.net";
        private readonly string CaseInvitationSubject = "You've Been Invited To Join A Case";

        public EmailController(IQueryFactory queryFactory, IMediator mediator)
        {
            _queryFactory = queryFactory;
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a case based on the situation.
        /// </summary>
        /// <param name="payload">The HTTP Post Payload, containing the situation and the case title.</param>
        /// <returns>The caseId of the newly created case</returns>
        [HttpPost, Route("sendToStaff")]
        public async Task<bool?> SendEmailToStaff([FromBody] EmailModel payload)
        {
            var email = new EmailNotification()
            {
                ToAddress = StaffAddress,
                Content = payload.Message,
                Subject = payload.Subject
            };

            var request = new SendStaffEmailCommand.Request()
            {
                EmailNotification = email
            };

            var result = await _mediator.SendAsync(request);

            return result.Result as bool?;
        }

        [HttpPost, Route("sendCaseInvitations")]
        public async Task<bool> SendCaseInvitations([FromBody] CaseInvitationModel payload)
        {
            var value = true;
            foreach (var address in payload.Addresses)
            {
                var email = new EmailNotification()
                {
                    ToAddress = address,
                    Content = GetCaseInvitationMessage(payload.CaseId),
                    Subject = CaseInvitationSubject
                };

                var request = new SendCaseInvitationsCommand.Request()
                {
                    EmailNotification = email
                };

                var result = await _mediator.SendAsync(request);

                if (!((bool) result.Result))
                {
                    value = false;
                }
            }

            return value;
        }

        private string GetCaseInvitationMessage(string caseId)
        {
            return caseId;
        }
    }
}