using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using MichaelsPlace.Extensions;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.CommandHandlers
{
    public interface ICommandResult
    {
        bool IsSuccess { get; }
        object Result { get; }
    }

    public class CommandResult : ICommandResult
    {
        public static ICommandResult Success(object result = null) => new CommandResult(true, result);

        public static ICommandResult Failure(object result = null) => new CommandResult(false, result);

        public static ICommandResult Merge(params ICommandResult[] results)
            => new CommandResult(results.All(r => r.IsSuccess), null);

        private CommandResult(bool success, object result)
        {
            IsSuccess = success;
            Result = result;
        }
        public bool IsSuccess { get; }
        public object Result { get; }
    }

    public class AddOrEditPersonCommand : IAsyncRequest<ICommandResult>
    {
        public ModelStateDictionary ModelState { get; set; }
        public PersonModel Person { get; set; }

        public AddOrEditPersonCommand(PersonModel person, ModelStateDictionary modelState)
        {
            Person = person;
            ModelState = modelState;
        }
    }

    public class CommandHandlerBase
    {
        private Injected<ApplicationDbContext> _dbContext;

        [Inject]
        public ApplicationDbContext DbContext
        {
            get { return _dbContext.Value; }
            set { _dbContext.Value = value; }
        }

        private Injected<IMapper> _mapper;

        [Inject]
        public IMapper Mapper
        {
            get { return _mapper.Value; }
            set { _mapper.Value = value; }
        }
    }

    public class AddOrEditPersonCommandHandler : CommandHandlerBase, IAsyncRequestHandler<AddOrEditPersonCommand, ICommandResult>
    {
        public async Task<ICommandResult> Handle(AddOrEditPersonCommand message)
        {
            if (message.Person.Id.IsPresent())
            {
                return Edit(message);
            }

            return Add(message);
        }
        
        private ICommandResult Add(AddOrEditPersonCommand message)
        {
            var person = Mapper.Map<Person>(message.Person);

            DbContext.People.Add(person);

            DbContext.SaveChanges();

            return CommandResult.Success(person.Id);
        }

        private ICommandResult Edit(AddOrEditPersonCommand message)
        {
            var person = DbContext.People.Include(p => p.ApplicationUser).First(u => u.Id == message.Person.Id);

            Mapper.Map(message.Person, person);

            DbContext.SaveChanges();

            return CommandResult.Success(person.Id);
        }
    }
}
