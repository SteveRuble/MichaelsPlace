using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using MediatR;
using MichaelsPlace.Handlers;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Services.Messaging;
using MichaelsPlace.Tests.TestHelpers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Moq;
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

                Bind<ApplicationRoleStore>().ToSelf().InSingletonScope();
                Bind<ApplicationRoleManager>().ToSelf().InSingletonScope();

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
        /// Configures mediator, but does not bind any handlers.
        /// </summary>
        public class Mediatr : NinjectModule
        {
            public override void Load()
            {
                Kernel.Bind(scan => scan.FromAssemblyContaining<IMediator>().SelectAllClasses().BindDefaultInterface());
                Bind<SingleInstanceFactory>().ToMethod(ctx => t => ctx.Kernel.Get(t));
                Bind<MultiInstanceFactory>().ToMethod(ctx => t =>
                {
                    var handlers = ctx.Kernel.GetAll(t);
                    return handlers;
                });
            }
        }

        /// <summary>
        /// Configures <see cref="IMediator"/> to be a mock, which can be obtained from the mocking kernal via GetMock.
        /// Loading this module will set the global mock behavior to <see cref="MockBehavior.Strict"/>,
        /// because otherwise awaiting async methods which haven't been set up will cause confusing null ref exceptions.
        /// </summary>
        public class MockMediatr : NinjectModule
        {
            public override void Load()
            {
                Kernel.Settings.SetMockBehavior(MockBehavior.Strict);
                Rebind<IMediator>().ToMock().InSingletonScope();
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
