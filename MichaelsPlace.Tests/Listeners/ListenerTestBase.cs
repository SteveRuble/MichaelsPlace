using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using Ninject;
using Ninject.Infrastructure;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    public abstract class ListenerTestBase<TListener> where TListener : IListener
    {
        public MoqMockingKernel Kernel { get; set; }

        public IMessageBus MessageBus => Kernel.Get<IMessageBus>();

        public IListener Target => Kernel.Get<IListener>();

        protected ListenerTestBase()
        {
            Kernel = new MoqMockingKernel();
            ((NinjectSettings)Kernel.Settings).DefaultScopeCallback = StandardScopeCallbacks.Singleton;
            Kernel.Bind<TListener>().ToSelf();
            Kernel.Bind<IMessageBus>().To<MessageBus>()
                .OnActivation((ctx, bus) => ctx.Kernel.Get<TListener>().SubscribeTo(bus));
            
        }

        [SetUp]
        public void SetUpBase()
        {
            Kernel.Reset();
        }

        public void WhenAMessageIsPublished<TMessage>(TMessage message)
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
