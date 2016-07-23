using System;
using System.Data.Entity;
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

            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, MichaelsPlace.Migrations.Configuration>());
            var context = new ApplicationDbContext();

            context.Database.Initialize(true);
        }
    }
}
