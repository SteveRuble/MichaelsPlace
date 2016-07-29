using System.Security.Claims;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MichaelsPlace.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MichaelsPlace.Models.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MichaelsPlace.Models.Persistence.ApplicationDbContext context)
        {
            SeedContext(context);

            base.Seed(context);
        }

        public static void SeedContext(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole() {Name = Constants.Roles.Administrator});
            var adminEmail = "admin@example.com";
            var user = userManager.FindByName(adminEmail);
            if (user == null)
            {
                user = new ApplicationUser()
                       {
                           UserName = "admin@example.com",
                           Email = "admin@example.com",
                           Person = new Person()
                                    {
                                        FirstName = "Admin",
                                        LastName = "Istrator",
                                    }
                       };
                var result = userManager.Create(user, "Administrator123!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(String.Join(";", result.Errors));
                }
                userManager.AddClaim(user.Id, new Claim(Constants.Claims.Staff, bool.TrueString));
            }
            try
            {
                if (!userManager.IsInRole(user.Id, Constants.Roles.Administrator))
                {
                    userManager.AddToRole(user.Id, Constants.Roles.Administrator);
                }
            }
            catch
            {
                // we'll need to run again.
            }
            var userShortage = 200 - context.Users.Count();
            for (int i = 0; i < userShortage; i++)
            {
                var randomUser = new ApplicationUser()
                                 {
                                     UserName = $"{SomeRandom.String()}@example.com",
                                     Email = $"{SomeRandom.String()}@example.com",
                                     Person = new Person()
                                              {
                                                  FirstName = SomeRandom.Name(),
                                                  LastName = SomeRandom.Name(),
                                                  EmailAddress = SomeRandom.EmailAddress(),
                                                  PhoneNumber = SomeRandom.PhoneNumber()
                                              }
                                 };
                userManager.Create(randomUser, SomeRandom.Name(20) + ".1");
            }

            var tb = new TagBuilder(context);
            if (!context.Tags.Any())
            {
                tb.CreateContextTag("Business", "Business", "A loss in a business context.")
                  .WithLosses("Employee", "Employee family member")
                  .WithRelationships("HR", "Employee", "Employee family member", "Owner", "Other");
                tb.CreateContextTag("Personal", "Personal", "A loss in a family context.")
                  .WithLosses("Child", "Parent", "Sibling", "Friend", "Other")
                  .WithRelationships("Parent", "Child", "Sibling", "Spouse", "Extended family", "Pastor", "Other");
                tb.CreateContextTag("Educational", "Educational", "A loss at a school or university.")
                  .WithLosses("Student", "Faculty", "Staff", "Family of student")
                  .WithRelationships("Administrator", "Faculty", "Student", "Family", "Other");
                tb.CreateContextTag("Organizational", "Church, sports team, or other organization", "A loss at a church or social organization.")
                  .WithLosses("Member", "Affiliate")
                  .WithRelationships("Member", "Leader");
            }

            for (int i = 1; i < 7; i++)
            {
                var contextTag = context.Tags.OfType<ContextTag>().OrderBy(t => t.Id%i).First();
                context.Items.AddOrUpdate(new Article()
                                          {
                                              Id = i,
                                              CreatedBy = user.UserName,
                                              CreatedUtc = DateTimeOffset.UtcNow,
                                              Content =
                                                  "Lorem ipsum dolor sit amet, sapien etiam, nunc amet dolor ac odio mauris justo. Luctus arcu, urna praesent at id quisque ac. Arcu es massa vestibulum malesuada, integer vivamus elit eu mauris eus, cum eros quis aliquam wisi. Nulla wisi laoreet suspendisse integer vivamus elit eu mauris hendrerit facilisi, mi mattis pariatur aliquam pharetra eget.",
                                              Title = "Article " + i,
                                              Order = i,
                                              AppliesToContexts = {contextTag},
                                              AppliesToRelationships = contextTag.Relationships.OrderBy(t => t.Id%i).Take(2).ToList(),
                                              AppliesToLosses = contextTag.Losses.OrderBy(t => t.Id%i).Take(2).ToList()
                                          });
            }

            for (int i = 1; i < 5; i++)
            {
                var contextTag = context.Tags.OfType<ContextTag>().OrderBy(t => t.Id%i).First();
                context.Items.AddOrUpdate(new ToDo()
                                          {
                                              Id = i + 7,
                                              CreatedBy = user.UserName,
                                              CreatedUtc = DateTimeOffset.UtcNow,
                                              Content =
                                                  "Luctus arcu, urna praesent at id quisque ac. Arcu es massa vestibulum malesuada, integer vivamus elit eu mauris eus.",
                                              Title = "ToDo " + i,
                                              Order = i,
                                              AppliesToContexts = {contextTag},
                                              AppliesToRelationships = contextTag.Relationships.OrderBy(t => t.Id%i).Take(2).ToList(),
                                              AppliesToLosses = contextTag.Losses.OrderBy(t => t.Id%i).Take(2).ToList()
                                          });
            }
        }

        public class TagBuilder
        {
            private readonly ApplicationDbContext _dbContext;
            private ContextTag _currentContext;
            private int _id = 0;

            public TagBuilder(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public TagBuilder CreateContextTag(string name, string display, string description)
            {
                _currentContext = new ContextTag() { Id = ++_id, Name = name, Description = description};
                _dbContext.Tags.AddOrUpdate(_currentContext);
                return this;
            }

            public TagBuilder WithLosses(params string[] names)
            {
                foreach (var name in names)
                {
                    var tag = _dbContext.Set<LossTag>().Create();
                    tag.Id = ++_id;
                    tag.Name = name;
                    tag.Context = _currentContext;
                    tag.ContextId = _currentContext.Id;
                    _currentContext.Losses.Add(tag);
                    _dbContext.Tags.AddOrUpdate(tag);

                }
                return this;
            }

            public TagBuilder WithRelationships(params string[] names)
            {
                foreach (var name in names)
                {
                    var tag = _dbContext.Set<RelationshipTag>().Create();
                    tag.Id = ++_id;
                    tag.Name = name;
                    tag.Context = _currentContext;
                    tag.ContextId = _currentContext.Id;
                    _currentContext.Relationships.Add(tag);
                    _dbContext.Tags.AddOrUpdate(tag);

                }
                return this;
            }
            
        }
        
    }
}
