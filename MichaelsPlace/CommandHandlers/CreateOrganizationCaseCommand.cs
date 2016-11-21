using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.CommandHandlers
{
    public class CreateOrganizationCaseCommand : CommandHandlerBase, IAsyncRequestHandler<CreateOrganizationCaseCommand.Request, ICommandResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public class Request : IAsyncRequest<ICommandResult>
        {
            public SituationModel Situation { get; set; }
            public string Title { get; set; }
            public int OrganizationId { get; set; }
        }

        public CreateOrganizationCaseCommand(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<ICommandResult> Handle(Request message)
        {
            var @case = new Case();

            var organization = (from organizations in _dbContext.Organizations
                                where organizations.Id == message.OrganizationId
                                select organizations).First();

            @case.Title = message.Title;
            @case.Organization = organization;
            @case.CaseItems = GetCaseItems(@case, message.Situation);
            @case.CaseUsers = GetCaseUsers(@case, organization, message.Situation);

            AddPersonCaseItems(@case, organization);

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

        private ICollection<PersonCase> GetCaseUsers(Case @case, Organization organization, SituationModel situation)
        {
            var caseUsers = new Collection<PersonCase>();

            var relationships = from relationship in _dbContext.Tags
                                where relationship.Id == situation.Relationships.FirstOrDefault()
                                select relationship;

            foreach (var person in organization.OrganizationPeople)
            {
                caseUsers.Add(new PersonCase
                {
                    Case = @case,
                    Person = person.Person,
                    Relationship = (RelationshipTag) relationships.FirstOrDefault()
                });
            }

            return caseUsers;
        }

        private void AddPersonCaseItems(Case @case, Organization organization)
        {
            foreach (var item in @case.CaseItems)
            {
                foreach (var person in organization.OrganizationPeople)
                {
                    var personCaseItem = new PersonCaseItem()
                    {
                        Person = person.Person,
                        Case = @case,
                        Item = item.Item
                    };

                    person.Person.PersonCaseItems.Add(personCaseItem);
                }
            }
        }
    }
}