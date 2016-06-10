using System.Web.Mvc;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Controllers
{
    public class ControllerIntegrationTestBase<T> : IntegrationTestBase
        where T : Controller
    {
        public T Target => MockingKernel.Get<T>();

        [SetUp]
        public void SetUpControllerIntegrationTestBase()
        {
            MockingKernel.Load<TestModules.Http>();
        }
    }
}