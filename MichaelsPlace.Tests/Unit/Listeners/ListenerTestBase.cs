using MediatR;
using MichaelsPlace.Models.Persistence;
using Ninject;
using Ninject.Infrastructure;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Listeners
{
    public abstract class ListenerTestBase<TListener, TNotification>
        where TListener : INotificationHandler<TNotification> 
        where TNotification : INotification
    {
        public MoqMockingKernel Kernel { get; set; }

        public IMediator MessageBus => Kernel.Get<IMediator>();

        public TListener Target => Kernel.Get<TListener>();

        protected ListenerTestBase()
        {
            Kernel = new MoqMockingKernel();
            ((NinjectSettings)Kernel.Settings).DefaultScopeCallback = StandardScopeCallbacks.Singleton;
            Kernel.Bind(typeof(TListener), typeof(INotificationHandler<TNotification>)).To<TListener>().InSingletonScope();
            Kernel.Load<TestModules.Mediatr>();

        }

        [SetUp]
        public void SetUpBase()
        {
            Kernel.Reset();
        }

        public void WhenAMessageIsPublished<TMessage>(TMessage message)
            where TMessage : INotification
        {
            MessageBus.Publish(message);
        }

        public void WhenAnEntityIsAdded<TEntity>(TEntity entity)
        {
            MessageBus.Publish(new EntityAdded<TEntity>(Kernel.Get<ApplicationDbContext>(), entity));
        }

        public void WhenAnEntityIsChanged<TEntity>(TEntity previous, TEntity current)
        {
            MessageBus.Publish(new EntityUpdating<TEntity>(Kernel.Get<ApplicationDbContext>(), previous, current));
        }
    }
}
