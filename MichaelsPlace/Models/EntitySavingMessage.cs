using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Infrastructure;

namespace MichaelsPlace.Models
{
    public static class MessageBusExtensions
    {
        public static void PublishEntityEntry(this IMessageBus messageBus, DbEntityEntry entityEntry)
        {
            
        }

    }
}
