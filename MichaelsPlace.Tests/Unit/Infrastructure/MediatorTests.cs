using FluentAssertions;
using MediatR;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Infrastructure
{
    [TestFixture]
    public class MediatorTests
    {
        public MoqMockingKernel Kernel { get; set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new MoqMockingKernel();
            Kernel.Load<TestModules.Mediatr>();
        }

        [Test]
        public void notifications()
        {
            var expected = new TestNotification();
            INotification actual = null;
            var mockHandler = new Mock<INotificationHandler<TestNotification>>();
            mockHandler.Setup(m => m.Handle(It.IsAny<TestNotification>())).Callback((INotification n) =>
            {
                actual = n;
            });

            Kernel.Bind<INotificationHandler<TestNotification>>().ToConstant(mockHandler.Object);

            var mediator = Kernel.Get<IMediator>();

            mediator.Publish(expected);

            actual.Should().BeSameAs(expected);
        }
    }

    public class TestNotification : INotification
    {
        
    }

}
