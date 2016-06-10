using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Infrastructure.Messaging;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Tests.TestHelpers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Factory;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Serilog;
using Serilog.Filters;

namespace MichaelsPlace.Tests
{
    public class TestModules
    {
        /// <summary>
        /// Binds <see cref="ApplicationDbContext"/> to a real instance with an 
        /// open <see cref="DbConnection"/> and <see cref="DbTransaction"/> already in place.
        /// The <see cref="DbConnection"/> and <see cref="DbTransaction"/> are bound as singletons
        /// and should be closed/disposed in the TearDown method of any test which uses this module.
        /// </summary>
        public class EntityFramework : NinjectModule
        {
            public override void Load()
            {
                var connection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                connection.Open();

                var transation = connection.BeginTransaction();

                Kernel.Bind<DbConnection>().ToConstant(connection);
                Kernel.Bind<DbTransaction>().ToConstant(transation);

                Kernel.Bind<ApplicationDbContext>().ToConstructor(syntax => new ApplicationDbContext(connection))
                      .OnActivation(db => db.Database.UseTransaction(transation));

                Kernel.Bind<IApplicationDbContextFactory>().ToFactory();
            }
        }

        /// <summary>
        /// Binds HTTP related components (<see cref="HttpContextBase"/>, <see cref="IOwinContext"/>) and
        /// authentication components to mocked singletons, except for <see cref="ApplicationUserManager" />
        /// which is bound to itself.
        /// </summary>
        public class Http : NinjectModule
        {
            public override void Load()
            {
                Bind<HttpContextBase>().ToMock().InSingletonScope();
                Kernel.MockingKernel().GetMock<HttpContextBase>().SetupAllProperties();
                Bind<IOwinContext>().ToMock().InSingletonScope();
                Kernel.MockingKernel().GetMock<IOwinContext>().SetupAllProperties();

                Bind<IAuthenticationManager>().ToMock().InSingletonScope();

                Bind<ApplicationUserManager>().ToMethod(ctx => new ApplicationUserManager(new ApplicationUserStore(ctx.Kernel.Get<ApplicationDbContext>())))
                                              .InSingletonScope();
            }
        }

        /// <summary>
        /// Logs to the console.
        /// </summary>
        public class Logging : NinjectModule
        {
            public override void Load()
            {
                var config = new LoggerConfiguration()
                    .WriteTo.ColoredConsole();

                var log = config.CreateLogger();

                Kernel.Bind<ILogger>().ToConstant(log);

            }
        }

        /// <summary>
        /// Configures message bus as a singleton with listeners subscribed, but does not bind any listeners.
        /// Any listeners which are bound when the message bus is first activated will be automatically subscribed.
        /// </summary>
        public class MessageBus : NinjectModule
        {
            public override void Load()
            {
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

        /// <summary>
        /// Binds all services to mocks.
        /// </summary>
        public class Services : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind<IEmailService>().ToMock().InSingletonScope();
                Kernel.Bind<ISmsService>().ToMock().InSingletonScope();
                Kernel.Bind<ISingleEntityService>().ToMock().InSingletonScope();
            }
        }
    }
    
}
