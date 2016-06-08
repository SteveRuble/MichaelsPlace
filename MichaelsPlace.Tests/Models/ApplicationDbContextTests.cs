using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Glimpse.Core.Extensions;
using MichaelsPlace.Models;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Models
{
    [TestFixture]
    public class ApplicationDbContextTests : IntegrationTestBase
    {
        [Test]
        public void can_create_user()
        {
            var user = new ApplicationUser()
                       {
                           Person = new Person()
                       };
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
            var set = DbContext.Set<EmailNotification>();
            set.Should().NotBeNull();
            set.Add(new EmailNotification()
                    {
                        Content = "Test",
                        CreatedBy = "test@example.com",
                        Subject = "Test Subject",
                        ToAddress = "test2@example.com"
                    });
            DbContext.SaveChanges(); 

        }

        [Test]
        public async Task changing_events_are_published()
        {
            var expected = new Case();
            DbContext.Cases.Add(expected);
            DbContext.SaveChanges();

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var actualTask = MessageBus.Observe<EntityUpdating<Case>>().FirstOrDefaultAsync().ToTask(cts.Token);

            expected.Title = "title";

            DbContext.SaveChanges();

            var actual = await actualTask;

            actual.Previous.Id.Should().Be(expected.Id);
            actual.Previous.Should().NotBeSameAs(expected);
            actual.Current.Should().BeSameAs(expected);
        }

        [Test]
        public async Task added_events_are_published()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var actualTask = MessageBus.Observe<EntityAdded<Case>>().FirstOrDefaultAsync().ToTask(cts.Token);

            var expected = new Case {Title = "title"};
            DbContext.Cases.Add(expected);
            DbContext.SaveChanges();
            
            var actual = await actualTask;

            actual.Entity.Should().Be(expected);
        }

        public Task<object> GetFirstAsync<T>()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            return MessageBus.Observe<T>().Select(x => (object) x).FirstOrDefaultAsync().ToTask(cts.Token);
        }
    }
}
