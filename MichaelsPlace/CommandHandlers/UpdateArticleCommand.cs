using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class UpdateArticleCommand : CommandHandlerBase, IAsyncRequestHandler<UpdateArticleCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public int Id { get; set; }
            public bool Status { get; set; }
            public string UserId { get; set; }
        }

        public UpdateArticleCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var person = (from people in _dbContext.People
                           where people.Id == message.UserId
                           select people).First();

            var article = person.PersonCaseItems.First(pci => pci.Id == message.Id);
            article.Status = message.Status ? CaseItemUserStatus.Viewed : CaseItemUserStatus.Assigned;

            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync();
        }
    }
}