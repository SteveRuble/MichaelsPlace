using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using MichaelsPlace.CommandHandlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Api;
using Microsoft.AspNet.Identity;

namespace MichaelsPlace.Controllers.Api
{
    [RoutePrefix("item")]
    public class ItemController : SpaApiControllerBase
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IMediator _mediator;

        public ItemController(IQueryFactory queryFactory, IMediator mediator)
        {
            _queryFactory = queryFactory;
            _mediator = mediator;
        }

        [HttpPost, Route("articles/updateStatus")]
        public async Task<ICommandResult> UpdateArticleStatus([FromBody] UpdateStatusModel payload)
        {
            var request = new UpdateArticleCommand.Request()
            {
                Id = payload.Id,
                Status = payload.Status,
                UserId = User.Identity.GetUserId()
            };

            return await _mediator.SendAsync(request);
        }

        [HttpPost, Route("todos/updateStatus")]
        public async Task<ICommandResult> UpdateTodoStatus([FromBody] UpdateStatusModel payload)
        {
            var request = new UpdateToDoCommand.Request()
            {
                Id = payload.Id,
                Status = payload.Status,
                CaseId = payload.CaseId
            };

            return await _mediator.SendAsync(request);
        }
    }
}
