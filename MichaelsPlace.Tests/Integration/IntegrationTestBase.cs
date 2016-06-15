using System.Data.Common;
using MediatR;
using MichaelsPlace.Models.Persistence;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration
{
    public class IntegrationTestBase
    {
        public IMediator Mediator { get; set; }

        public ApplicationDbContext DbContext { get; set; }

        public MoqMockingKernel MockingKernel { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            MockingKernel = new MoqMockingKernel();

            MockingKernel.Load<Modules.Mapping>();

            MockingKernel.Load<TestModules.Logging>();
            MockingKernel.Load<TestModules.Mediatr>();
            MockingKernel.Load<TestModules.EntityFramework>();

            Mediator = MockingKernel.Get<IMediator>();

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