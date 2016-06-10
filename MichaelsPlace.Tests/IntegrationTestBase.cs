using System;
using System.Data;
using System.Data.Common;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;
using Serilog;

namespace MichaelsPlace.Tests
{
    public class IntegrationTestBase
    {
        public IMessageBus MessageBus { get; set; }

        public ApplicationDbContext DbContext { get; set; }

        public MoqMockingKernel MockingKernel { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            MockingKernel = new MoqMockingKernel();

            MockingKernel.Load<Modules.Mapping>();

            MockingKernel.Load<TestModules.Logging>();
            MockingKernel.Load<TestModules.MessageBus>();
            MockingKernel.Load<TestModules.EntityFramework>();

            MessageBus = MockingKernel.Get<IMessageBus>();

            DbContext = MockingKernel.Get<ApplicationDbContext>();
        }

        [TearDown]
        public void TearDownbase()
        {
            MockingKernel.Get<DbConnection>().Close();
            MockingKernel.Get<DbTransaction>().Dispose();
        }
    }
}