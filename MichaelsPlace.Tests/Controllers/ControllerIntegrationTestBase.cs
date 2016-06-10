using System.Web.Mvc;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using Ninject;
using Ninject.MockingKernel.Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests
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