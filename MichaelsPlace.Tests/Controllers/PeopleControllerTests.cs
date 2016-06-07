using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MichaelsPlace.Controllers.Admin;
using MichaelsPlace.Models.Persistence;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Controllers
{
    [TestFixture]
    public class PeopleControllerTests
    {
        [Test]
        public void get_index()
        {
            var kernel = new StandardKernel(new MichaelsPlaceModule());
            var target = kernel.Get<PeopleController>();

            var result = target.Index() as ViewResult;
        }
    }
}
