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
        public async Task changed_events_are_published()
        {
            var expected = new Case();
            DbContext.Cases.Add(expected);
            DbContext.SaveChanges();

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            var actualTask = MessageBus.Observe<EntityChanging<Case>>().FirstOrDefaultAsync().ToTask(cts.Token);

            expected.Title = "title";

            DbContext.SaveChanges();

            var actual = await actualTask;

            actual.Previous.Id.Should().Be(expected.Id);
            actual.Previous.Should().NotBeSameAs(expected);
            actual.Current.Should().BeSameAs(expected);
        }

        public static IEnumerable<TestCaseData> PublishedEntities()
        {
            yield return new TestCaseData(new Case()).SetName("Case");
            yield return new TestCaseData(new Item() { Title = "title", Content = "content"}).SetName("Item");
            yield return new TestCaseData(new Organization()).SetName("Organization");
            yield return new TestCaseData(new Notification() { Content = "content"}).SetName("Notification");
            yield return new TestCaseData(new HistoricalEvent()
                                          {
                                              Id = Guid.NewGuid().ToString(),
                                              ContentJson = "{}",
                                              EventType = "string",
                                              TimestampUtc = DateTimeOffset.UtcNow,
                                          }).SetName("HistoricalEvent");

        }

        [TestCaseSource("PublishedEntities")]
        public async Task adds_are_published(object entity)
        {
            var entityType = entity.GetType();
            var set = DbContext.Set(entityType);
            var method = typeof(ApplicationDbContextTests).GetMethod(nameof(GetFirstAsync));
            var genericMethod = method.MakeGenericMethod(typeof(EntityAdded<>).MakeGenericType(entityType));
            var actualTask = genericMethod.Invoke(this, new object[0]) as Task<object>;

            set.Add(entity);

            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var actualEntry = actualTask.Result;
            var expectedType = typeof(EntityAdded<>).MakeGenericType(entityType);
            actualEntry.Should().BeOfType(expectedType);
            var propertyInfo = expectedType.GetProperty(nameof(EntityAdded<string>.Entity));
            var actualEntity = propertyInfo.GetValue(actualEntry);
            actualEntity.Should().Be(entity);
        }

        public Task<object> GetFirstAsync<T>()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            return MessageBus.Observe<T>().Select(x => (object) x).FirstOrDefaultAsync().ToTask(cts.Token);
        }
    }
}
