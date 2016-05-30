using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Internal;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;

namespace MichaelsPlace
{
    public partial class Startup
    {
        private void ConfigureData(IAppBuilder app)
        {
            Database.SetInitializer(new DevDatabaseInitializer()
                                    {
                                        //RecreateDatabase = true,
                                    });
        }
    }

    public class DevDatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        public bool RecreateDatabase { get; set; }

        public bool ReseedDatabase { get; set; }

        public override void InitializeDatabase(ApplicationDbContext context)
        {
            var modelChanged = !context.Database.CompatibleWithModel(true);
            if (!RecreateDatabase && !ReseedDatabase && !modelChanged)
            {
                return;
            }

            if (RecreateDatabase || modelChanged)
            {
                context.Database.Delete();
                context.Database.Create();
            }
            if (ReseedDatabase || RecreateDatabase || modelChanged)
            {
                this.Seed(context);
                context.SaveChanges();
            }
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            roleManager.Create(new IdentityRole() {Name = Constants.Roles.Administrator});
            var adminEmail = "admin@example.com";
            var user = userManager.FindByEmail(adminEmail);
            if (user == null)
            {
                user = new ApplicationUser()
                           {
                               UserName = "admin@example.com"
                           };
                userManager.Create(user, "Administrator123");
            }
            if (!userManager.IsInRole(user.Id, Constants.Roles.Administrator))
            {
                userManager.AddToRole(user.Id, Constants.Roles.Administrator);
            }
            
            context.Tags.Add(new DemographicTag() { Title = "Friend", GuidanceLabel = "I'm helping a friend or relative" });
            context.Tags.Add(new DemographicTag() { Title = "Person", GuidanceLabel = "I've suffered a loss" });
            context.Tags.Add(new DemographicTag() { Title = "School Administrator", GuidanceLabel = "I'm a school administrator" });
            context.Tags.Add(new DemographicTag() { Title = "Manager", GuidanceLabel = "I manage a workplace" });

            context.Tags.Add(new LossTag() { Title="Spouse", GuidanceLabel = "I lost a spouse" });
            context.Tags.Add(new LossTag() { Title="Parent", GuidanceLabel = "I lost a parent" });
            context.Tags.Add(new LossTag() { Title="Child", GuidanceLabel = "I lost a child" });
            context.Tags.Add(new LossTag() { Title="Friend", GuidanceLabel = "I lost a friend" });

            context.Tags.Add(new MournerTag() { Title="Siblings", GuidanceLabel = "My siblings" });
            context.Tags.Add(new MournerTag() { Title="Classmates", GuidanceLabel = "My classmates" });
            context.Tags.Add(new MournerTag() { Title="Employees", GuidanceLabel = "My employees" });
            context.Tags.Add(new MournerTag() { Title="Self", GuidanceLabel = "Myself" });

            var situation = context.Situations.Add(new Situation()
                                                   {
                                                       Losses = context.Tags.Local.OfType<LossTag>().ToList(),
                                                       Mourners = context.Tags.Local.OfType<MournerTag>().ToList(),
                                                       Demographics = context.Tags.Local.OfType<DemographicTag>().ToList(),
                                                   });

            for (int i = 1; i < 7; i++)
            {
                context.Items.Add(new Article()
                                  {
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
                context.Items.Add(new ToDo()
                                  {
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
