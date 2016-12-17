using System.Threading.Tasks;
using MediatR;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class SendStaffEmailCommand : CommandHandlerBase, IAsyncRequestHandler<SendStaffEmailCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public EmailNotification EmailNotification { get; set; }
        }

        public SendStaffEmailCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            _dbContext.Notifications.Add(message.EmailNotification);
            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(true);
        }
    }
}