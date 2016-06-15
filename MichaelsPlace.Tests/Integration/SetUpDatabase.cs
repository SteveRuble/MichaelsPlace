using System;
using System.IO;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Integration
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
