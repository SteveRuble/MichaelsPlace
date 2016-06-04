using System;
using MichaelsPlace.Infrastructure;
using MichaelsPlace.Models.Persistence;
using NUnit.Framework;
using Serilog;

namespace MichaelsPlace.Tests
{
    public class DatabaseIntegrationTestBase
    {
        public IDisposable Transaction { get; set; }
        public MessageBus MessageBus { get; set; }
        public ApplicationDbContext DbContext { get; set; }

        [SetUp]
        public void SetUpDatabase()
        {
            MessageBus = new MessageBus(Log.Logger);
            DbContext = new ApplicationDbContext(MessageBus);
            Transaction = DbContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDownDatabase()
        {
            Transaction?.Dispose();
        }

        
    }
}