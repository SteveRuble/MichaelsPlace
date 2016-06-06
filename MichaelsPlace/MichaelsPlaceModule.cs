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
using Ninject.Modules;
using Serilog;

namespace MichaelsPlace
{
    /// <summary>
    /// Configures dependency injection.
    /// </summary>
    public class MichaelsPlaceModule : NinjectModule
    {
        private readonly bool _logToConsole;

        /// <summary>
        /// Default constructor, configures logging normally.
        /// </summary>
        public MichaelsPlaceModule() : this(false) { }

        /// <summary>
        /// Extra constructor to configure logging for the test environment.
        /// </summary>
        /// <param name="logToConsole"></param>
        public MichaelsPlaceModule(bool logToConsole)
        {
            _logToConsole = logToConsole;
        }

        public override void Load()
        {
            ConfigureLogging();               
            
            Kernel.Bind(c => c.FromThisAssembly().SelectAllClasses().BindDefaultInterface());

            Kernel.Bind(c => c.FromThisAssembly()
                              .SelectAllClasses()
                              .InheritedFrom<Profile>()
                              .BindToSelf());

            Kernel.Bind<IMapper>().ToMethod(CreateMapper).InSingletonScope();

            Kernel.Bind<ApplicationDbContext>().ToConstructor(syntax => new ApplicationDbContext(syntax.Inject<IMessageBus>()));

            ConfigureHelpers();

            ConfigureHttp();

            // This configuration should be run last.
            ConfigureMessageBus(Kernel);

        }

        private void ConfigureHelpers()
        {
            Bind<SubscriptionHelper>().To<SubscriptionHelper>().InSingletonScope();
        }

        private void ConfigureLogging()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log-{Date}.log");

            var config = new LoggerConfiguration()
                .ReadFrom.AppSettings();
            if (_logToConsole)
            {
                config = config.WriteTo.ColoredConsole();
            }
            else
            {
                config = config.WriteTo.RollingFile(path, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 10)
                               .WriteTo.Glimpse();
            }

            var log = config.CreateLogger();

            Bind<ILogger>().ToConstant(log);
        }

        private void ConfigureHttp()
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
        /// <param name="kernel"></param>
        private static void ConfigureMessageBus(IKernel kernel)
        {
            var messageBus = new MessageBus(kernel.Get<ILogger>());

            kernel.Rebind<IMessageBus>().ToConstant(messageBus);

            kernel.Bind(c => c.FromAssemblyContaining<IListener>()
                              .SelectAllClasses()
                              .InheritedFrom<IListener>()
                              .BindAllInterfaces()
                              .Configure(ca => ca.InSingletonScope()));

            var listeners = kernel.GetAll<IListener>();
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
