using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class UpdateToDoCommand : CommandHandlerBase, IAsyncRequestHandler<UpdateToDoCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public int Id { get; set; }
            public bool Status { get; set; }
            public string CaseId { get; set; }
        }

        public UpdateToDoCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var currentCase = (from cases in _dbContext.Cases
                               where cases.Id == message.CaseId
                               select cases).First();

            var todo = currentCase.CaseItems.First(ci => ci.Id == message.Id);
            todo.Status = message.Status ? CaseItemStatus.Closed : CaseItemStatus.Open;

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync();
        }
    }
}