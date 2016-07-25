using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Tests.Integration;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration.Models
{
    [TestFixture]
    public class ApplicationDbContextTests : IntegrationTestBase
    {
        public Mock<IMediator> MockMediator { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockMediator = new Mock<IMediator>();
            Mediator = MockMediator.Object;
            DbContext.Mediator = Mediator;
        }

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

            expected.Title = "title";

            DbContext.SaveChanges();

            MockMediator.Verify(m => m.Publish(It.Is<EntityUpdating<Case>>(e => e.Previous.Id == expected.Id
                                                                                && e.Previous != expected
                                                                                && e.Current == expected)));
        }

        [Test]
        public async Task added_events_are_published()
        {
       
            var expected = new Case {Title = "title"};
            DbContext.Cases.Add(expected);
            DbContext.SaveChanges();

            MockMediator.Verify(m => m.Publish(It.Is<EntityAdded<Case>>(e => e.Entity == expected)));
        }

        [Test]
        public void soft_delete_hides_entity()
        {
            var item = new Item() {Title = "test", Content = "content"};
            DbContext.Items.Add(item);
            DbContext.SaveChanges();

            var retrievedItem = DbContext.Items.First(i => i.Id == item.Id);
            retrievedItem.Should().NotBeNull();

            DbContext.Items.Remove(retrievedItem);
            DbContext.SaveChanges();

            var deletedItem = DbContext.Items.FirstOrDefault(i => i.Id == item.Id);
            deletedItem.Should().BeNull();

            DbContext.SoftDeleteControl.Enabled = false;

            var softDeletedItem = DbContext.Items.FirstOrDefault(i => i.Id == item.Id);
            softDeletedItem.Should().NotBeNull();
        }
    }
}
