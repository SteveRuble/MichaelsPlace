using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Models
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        public ApplicationDbContext Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            Database.SetInitializer(new DevDatabaseInitializer()
                                    {
                                       RecreateDatabase = true
                                    });

            Target = new ApplicationDbContext();
        }

        [Test]
        public void can_create_user()
        {
            var user = new ApplicationUser();
            user.UserName = "test@example.com";

            Target.Users.Add(user);

            Target.SaveChanges();

            user.Id.Should().NotBeNullOrEmpty();

            Target.Users.Remove(user);

            Target.SaveChanges();
        }
    }
}
