using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ninject;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    [TestFixture]
    public class MappingValidation
    {
        [Test]
        public void mapping_is_valid()
        {
            var kernel = new StandardKernel();
            kernel.Load(new MichaelsPlaceModule(true));

            var mapper = kernel.Get<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
