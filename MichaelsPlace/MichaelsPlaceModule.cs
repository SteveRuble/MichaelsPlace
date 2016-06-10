using System;
using System.IO;
using System.Web;
using AutoMapper;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services;
using MichaelsPlace.Services.Messaging;
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
    /// Container class for <see cref="NinjectModule"/> implementations,
    /// so that they can be concise and re-usable without being scattered all over the application.
    /// </summary>
    public class Modules
    {
        public class EntityFramework : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<ApplicationDbContext>().ToSelf().WhenInjectedInto<SingleEntityService>();
                Kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();

                Kernel.Bind<IApplicationDbContextFactory>().ToFactory();
            }
        }

        public class Http : NinjectModule
        {
            public override void Load()
            {
                Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current));
                Bind<IOwinContext>().ToMethod(ctx => HttpContext.Current.GetOwinContext());

                Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
                Bind<ApplicationUserManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>());

            }
        }

        public class Logging : NinjectModule
        {
            public override void Load()
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
        }

        public class MessageBus : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(c => c.FromAssemblyContaining<IListener>()
                     .SelectAllClasses()
                     .InheritedFrom<IListener>()
                     .BindAllInterfaces()
                     .Configure(ca => ca.InSingletonScope()));

                Kernel.Bind<IMessageBus>()
                      .To<MichaelsPlace.Infrastructure.MessageBus>()
                      .InSingletonScope()
                      .OnActivation((ctx, bus) =>
                      {
                          var listeners = ctx.Kernel.GetAll<IListener>();
                          foreach (var listener in listeners)
                          {
                              listener.SubscribeTo(bus);
                          }
                      });
            }
        }

        public class Mapping : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(c => c.FromThisAssembly()
                  .SelectAllClasses()
                  .InheritedFrom<Profile>()
                  .BindToSelf());


                Kernel.Bind<IMapper>().ToMethod(CreateMapper).InSingletonScope();
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

        public class Services : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<IEmailService>().To<DevelopmentEmailService>().InSingletonScope();
                Kernel.Bind<ISmsService>().To<DevelopmentSmsService>().InSingletonScope();
                Kernel.Bind<ISingleEntityService>().To<SingleEntityService>().InSingletonScope();
            }
        }
    }
}
