using System.Collections.Generic;
using System.Linq;
using MichaelsPlace.Handlers;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using Moq;
using Ninject.MockingKernel;
using NUnit.Framework;
using TestStack.BDDfy;

namespace MichaelsPlace.Tests.Unit.Listeners
{
    [TestFixture]
    public class CaseAddedListenerTests : ListenerTestBase<CaseAddedListener, EntityAdded<Case>>
    {
        public Mock<ISingleEntityService> MockEntitySaver => Kernel.GetMock<ISingleEntityService>();
        public Mock<PreferencesQuery> MockPreferencesQuery => Kernel.GetMock<PreferencesQuery>();

        [SetUp]
        public void SetUp()
        {
            Kernel.Bind<PreferencesQuery>().ToMock();
        }
        
        public void GivenASubscription(string name, bool isEmailRequested, bool isSmsRequested)
        {
            var result = new List<SubscriptionPreferenceDetails>()
                         {
                             new SubscriptionPreferenceDetails()
                             {
                                 EmailAddress = TestConstants.Email,
                                 IsSmsRequested = isSmsRequested,
                                 IsEmailRequested = isEmailRequested,
                                 PhoneNumber = TestConstants.PhoneNumber,
                             }
                         };
            MockPreferencesQuery.Setup(m => m.GetSubscriptionPreferenceDetails(name)).Returns(result.AsQueryable());
        }

        [Test]
        public void message_is_received()
        {
            this.Given(x => x.GivenASubscription(Constants.Subscriptions.UserCaseCreated, true, true))
                .When(x => x.WhenAnEntityIsAdded(new Case()
                                                 {
                                                     Id = TestConstants.IdA
                                                 }))
                .Then(x => MockEntitySaver.Verify(m => m.Save(It.Is<EmailNotification>(e => e.ToAddress == TestConstants.Email))),
                      "Then an email notification should be saved.")
                .BDDfy();
        }

    }
}