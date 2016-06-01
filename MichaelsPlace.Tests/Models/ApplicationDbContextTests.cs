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
    public class ApplicationDbContextTests : DatabaseIntegrationTestBase
    {
        [Test]
        public void can_create_user()
        {
            var user = new ApplicationUser();
            user.UserName = "test@example.com";

            DbContext.Users.Add(user);

            DbContext.SaveChanges();

            user.Id.Should().NotBeNullOrEmpty();

            DbContext.Users.Remove(user);

            DbContext.SaveChanges();
        }

        [Test]
        public void can_get_set_for_derived_type()
        {
            var set = DbContext.Set<Email>();
            set.Should().NotBeNull();
            set.Add(new Email()
                    {
                        Content = "Test",
                        CreatedBy = "test@example.com",
                        Subject = "Test Subject",
                        ToAddress = "test2@example.com"
                    });
            DbContext.SaveChanges(); 

        }
    }
}
