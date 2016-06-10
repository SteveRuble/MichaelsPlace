using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Persistence;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Extensions.Factory;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using Serilog;
using Serilog.Filters;

namespace MichaelsPlace.Tests
{
    /// <summary>
    /// <para>
    /// Module which includes the bindings from <see cref="MichaelsPlaceModule"/>,
    /// but rebinds some context-dependent component to mocks or in a non-context-dependent way in singleton scope 
    /// Components which depend on the request are mocked, while components that have a per-request scope
    /// are rebound as singletons.
    /// </para>
    /// <para>
    /// This module does not wire up the message bus listeners.
    /// </para>
    /// </summary>
    public class DatabaseIntegrationTestModule : MichaelsPlaceModule
    {
        public MoqMockingKernel MockingKernel => (MoqMockingKernel) Kernel;

        protected override void ConfigureEntityFramework()
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

        protected override void ConfigureLogging()
        {
            ConfigureLogging(Kernel);
        }

        public static void ConfigureLogging(IKernel kernel)
        {
            var config = new LoggerConfiguration()
                .WriteTo.ColoredConsole();

            var log = config.CreateLogger();

            kernel.Bind<ILogger>().ToConstant(log);
        }

        protected override void ConfigureHttp()
        {
            Bind<HttpContextBase>().ToMock().InSingletonScope();
            Bind<IOwinContext>().ToMock().InSingletonScope();
            MockingKernel.GetMock<IOwinContext>().SetupAllProperties();

            Bind<IAuthenticationManager>().ToMock().InSingletonScope();

            Bind<ApplicationUserManager>().ToConstant(new ApplicationUserManager(new ApplicationUserStore(MockingKernel.Get<ApplicationDbContext>())));
        }

        protected override void ConfigureMessageBus()
        {
            var messageBus = new MessageBus(Kernel.Get<ILogger>());

            Rebind<IMessageBus>().ToConstant(messageBus);
        }
    }
}
