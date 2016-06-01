using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    [SetUpFixture]
    public class SetUpDatabase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
        }
    }
}
