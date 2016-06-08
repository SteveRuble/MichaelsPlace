using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
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

            var context = new ApplicationDbContext();
            context.Database.Initialize(true);
        }
    }
}
