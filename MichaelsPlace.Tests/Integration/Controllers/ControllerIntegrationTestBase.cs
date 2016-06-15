using System.Web.Mvc;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration.Controllers
{
    public class ControllerIntegrationTestBase<T> : IntegrationTestBase
        where T : Controller
    {
        public T Target => MockingKernel.Get<T>();

        [SetUp]
        public void SetUpControllerIntegrationTestBase()
        {
            MockingKernel.Load<TestModules.Http>();
            MockingKernel.Load<TestModules.MockMediatr>();
        }
    }
}