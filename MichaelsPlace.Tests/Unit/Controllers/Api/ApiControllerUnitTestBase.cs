using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Controllers.Api
{
    public class ApiControllerUnitTestBase<TController>
    {
        public MoqMockingKernel Kernel { get; set; }

        public TController Target => Kernel.Get<TController>();

        [SetUp]
        public void SetUp()
        {
            Kernel = new MoqMockingKernel();
            Kernel.Settings.SetMockBehavior(MockBehavior.Strict);
            Kernel.Load<TestModules.MockedQueries>();
            Kernel.Bind<TController>().ToSelf().InSingletonScope();
        }
    }
}