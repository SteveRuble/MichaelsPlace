using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.MockingKernel.Moq;

namespace MichaelsPlace.Tests.TestHelpers
{
    public static class KernelExtensions
    {
        public static MoqMockingKernel MockingKernel(this IKernel @this) => (MoqMockingKernel)@this;
    }
}
