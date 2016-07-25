using System;
using System.Data.Entity;
using System.IO;
using MichaelsPlace.Migrations;
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
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            
        }
    }
}
