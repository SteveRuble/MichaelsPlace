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

    internal sealed class Configuration : DbMigrationsConfiguration<MichaelsPlace.Models.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MichaelsPlace.Models.Persistence.ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole() { Name = Constants.Roles.Administrator });
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


            context.Tags.AddOrUpdate(new DemographicTag() { Id = 1, Title = "Friend", GuidanceLabel = "I'm helping a friend or relative" });
            context.Tags.AddOrUpdate(new DemographicTag() { Id = 2, Title = "Person", GuidanceLabel = "I've suffered a loss" });
            context.Tags.AddOrUpdate(new DemographicTag() { Id = 3, Title = "School Administrator", GuidanceLabel = "I'm a school administrator" });
            context.Tags.AddOrUpdate(new DemographicTag() { Id = 4, Title = "Manager", GuidanceLabel = "I manage a workplace" });

            context.Tags.AddOrUpdate(new LossTag() { Id = 5, Title = "Spouse", GuidanceLabel = "I lost a spouse" });
            context.Tags.AddOrUpdate(new LossTag() { Id = 6, Title = "Parent", GuidanceLabel = "I lost a parent" });
            context.Tags.AddOrUpdate(new LossTag() { Id = 7, Title = "Child", GuidanceLabel = "I lost a child" });
            context.Tags.AddOrUpdate(new LossTag() { Id = 8, Title = "Friend", GuidanceLabel = "I lost a friend" });

            context.Tags.AddOrUpdate(new MournerTag() { Id = 9, Title = "Siblings", GuidanceLabel = "My siblings" });
            context.Tags.AddOrUpdate(new MournerTag() { Id = 10, Title = "Classmates", GuidanceLabel = "My classmates" });
            context.Tags.AddOrUpdate(new MournerTag() { Id = 11, Title = "Employees", GuidanceLabel = "My employees" });
            context.Tags.AddOrUpdate(new MournerTag() { Id = 12, Title = "Self", GuidanceLabel = "Myself" });

            var situation = new Situation()
            {
                Id = 1,
                Losses = context.Tags.Local.OfType<LossTag>().ToList(),
                Mourners = context.Tags.Local.OfType<MournerTag>().ToList(),
                Demographics = context.Tags.Local.OfType<DemographicTag>().ToList(),
            };
            context.Situations.AddOrUpdate(situation);

            for (int i = 1; i < 7; i++)
            {
                context.Items.AddOrUpdate(new Article()
                {
                    Id = i,
                    CreatedBy = user.UserName,
                    CreatedUtc = DateTimeOffset.UtcNow,
                    Content =
                                          "Lorem ipsum dolor sit amet, sapien etiam, nunc amet dolor ac odio mauris justo. Luctus arcu, urna praesent at id quisque ac. Arcu es massa vestibulum malesuada, integer vivamus elit eu mauris eus, cum eros quis aliquam wisi. Nulla wisi laoreet suspendisse integer vivamus elit eu mauris hendrerit facilisi, mi mattis pariatur aliquam pharetra eget.",
                    Title = "Article " + i,
                    Order = i,
                    Situations =
                                      {
                                          situation
                                      }
                });

            }

            for (int i = 1; i < 5; i++)
            {
                context.Items.AddOrUpdate(new ToDo()
                {
                    Id = i + 7,
                    CreatedBy = user.UserName,
                    CreatedUtc = DateTimeOffset.UtcNow,
                    Content =
                                                  "Luctus arcu, urna praesent at id quisque ac. Arcu es massa vestibulum malesuada, integer vivamus elit eu mauris eus.",
                    Title = "ToDo " + i,
                    Order = i,
                    Situations =
                                              {
                                                  situation
                                              }
                });

            }

            base.Seed(context);

        }
    }
}
