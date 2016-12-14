using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class CreateCaseCommand : CommandHandlerBase, IAsyncRequestHandler<CreateCaseCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public SituationModel Situation { get; set; }
            public string Title { get; set; }
            public string UserId { get; set; }
        }

        public CreateCaseCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var person = (from people in _dbContext.People
                         where people.Id == message.UserId
                         select people).First();

            var @case = new Case();
            @case.Title = message.Title;
            @case.CaseItems = GetCaseItems(@case, message.Situation);
            @case.CaseUsers = GetCaseUsers(@case, person, message.Situation);

            var personCaseItems = GetPersonCaseItems(@case, person);

            foreach (var pci in personCaseItems)
            {
                person.PersonCaseItems.Add(pci);
            }

            _dbContext.Cases.Add(@case);
            _dbContext.SaveChanges();

            return CommandResult.SuccessAsync(@case.Id);
        }

        private ICollection<CaseItem> GetCaseItems(Case @case, SituationModel situation)
        {
            var collection = new Collection<CaseItem>();

            var items = from item in _dbContext.Items
                        where item.AppliesToContexts.Any(c => situation.Contexts.Contains(c.Id))
                                && item.AppliesToLosses.Any(c => situation.Losses.Contains(c.Id))
                                && item.AppliesToRelationships.Any(c => situation.Relationships.Contains(c.Id))
                        select item;

            foreach (var item in items)
            {
                var caseItem = new CaseItem
                {
                    Case = @case,
                    Item = item,
                };

                collection.Add(caseItem);
            }

            return collection;
        }

        private ICollection<PersonCase> GetCaseUsers(Case @case, Person person, SituationModel situation)
        {
            var caseUsers = new Collection<PersonCase>();

            var relationships = from relationship in _dbContext.Tags
                                where relationship.Id == situation.Relationships.FirstOrDefault()
                                select relationship;

            caseUsers.Add(new PersonCase
            {
                Case = @case,
                Person = person,
                Relationship = (RelationshipTag) relationships.FirstOrDefault(),
                IsOwner = true
            });

            return caseUsers;
        }

        private ICollection<PersonCaseItem> GetPersonCaseItems(Case @case, Person person)
        {
            var collection = new Collection<PersonCaseItem>();

            foreach (var item in @case.CaseItems)
            {
                var personCaseItem = new PersonCaseItem()
                {
                    Person = person,
                    Case = @case,
                    Item = item.Item
                };

                collection.Add(personCaseItem);
            }

            return collection;
        }
    }
}