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

        [HttpGet, Route("articles/updateStatus/{id:int}/{status:bool}/{caseId}")]
        public async Task<ICommandResult> UpdateArticleStatus(int id, bool status, string caseId)
        {
            var request = new UpdateArticleCommand.Request()
            {
                Id = id,
                Status = status,
                UserId = User.Identity.GetUserId()
            };

            return await _mediator.SendAsync(request);
        }

        [HttpGet, Route("todos/updateStatus/{id:int}/{status:bool}/{caseId}")]
        public async Task<ICommandResult> UpdateTodoStatus(int id, bool status, string caseId)
        {
            var request = new UpdateToDoCommand.Request()
            {
                Id = id,
                Status = status,
                CaseId = caseId
            };

            return await _mediator.SendAsync(request);
        }
    }
}
