using System;
using System.IO;
using System.Web;
using AutoMapper;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Subscriptions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;
using Serilog;
using Serilog.Filters;

namespace MichaelsPlace
{
    /// <summary>
    /// Configures dependency injection.
    /// </summary>
    public class MichaelsPlaceModule : NinjectModule
    {
        public override void Load()
        {
            ConfigureLogging();               
            
            Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindDefaultInterface());

            Kernel.Bind(c => c.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom<Profile>()
                              .BindToSelf());

            Kernel.Bind<IMapper>().ToMethod(CreateMapper).InSingletonScope();

            ConfigureEntityFramework();

            ConfigureHelpers();

            ConfigureHttp();

            // This configuration should be run last.
            ConfigureMessageBus();

        }

        protected virtual void ConfigureEntityFramework()
        {
            Kernel.Bind<ApplicationDbContext>().ToSelf().WhenInjectedInto<EntitySaver>();
            Kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();

            Kernel.Bind<IApplicationDbContextFactory>().ToFactory();
        }

        protected virtual void ConfigureHelpers()
        {
            Bind<SubscriptionHelper>().To<SubscriptionHelper>().InSingletonScope();
        }

        protected virtual void ConfigureLogging()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log-{Date}.log");

            var config = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .Filter.ByExcluding(Matching.FromSource(typeof(ApplicationDbContext).FullName))
                .WriteTo.RollingFile(path, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 10)
                .WriteTo.Glimpse();
            

            var log = config.CreateLogger();

            Bind<ILogger>().ToConstant(log);
        }

        protected virtual void ConfigureHttp()
        {
            Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current));
            Bind<IOwinContext>().ToMethod(ctx => HttpContext.Current.GetOwinContext());

            Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
            Bind<ApplicationUserManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>());

        }

        /// <summary>
        /// Configures the <see cref="IMessageBus"/> for this <paramref name="kernel"/>
        /// by subscribing all <see cref="IListener"/> implementations to the singleton <see cref="IMessageBus"/>.
        /// </summary>
        protected virtual void ConfigureMessageBus()
        {
            var messageBus = new MessageBus(Kernel.Get<ILogger>());

            Kernel.Rebind<IMessageBus>().ToConstant(messageBus);

            Kernel.Bind(c => c.FromAssemblyContaining<IListener>()
                              .SelectAllClasses()
                              .InheritedFrom<IListener>()
                              .BindAllInterfaces()
                              .Configure(ca => ca.InSingletonScope()));

            var listeners = Kernel.GetAll<IListener>();
            foreach (var listener in listeners)
            {
                listener.SubscribeTo(messageBus);
            }
        }

        private static IMapper CreateMapper(IContext ctx)
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ApiModelMappingProfile>();
                c.AddProfile<AdminModelMappingProfile>();
            });
            return configuration.CreateMapper();
        }
    }
}
