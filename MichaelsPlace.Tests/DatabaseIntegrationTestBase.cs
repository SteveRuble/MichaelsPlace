using System;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;

namespace MichaelsPlace.Tests
{
    public class DatabaseIntegrationTestBase
    {
        public IDisposable Transaction { get; set; }
        public ApplicationDbContext DbContext { get; set; }

        [SetUp]
        public void SetUpDatabase()
        {
            DbContext = new ApplicationDbContext();
            Transaction = DbContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDownDatabase()
        {
            Transaction?.Dispose();
        }

        
    }
}