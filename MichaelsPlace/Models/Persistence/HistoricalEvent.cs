using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Infrastructure;
using Newtonsoft.Json;

namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    /// Base class for persisted events, which represent important changes 
    /// in cases or user situations. Events 
    /// </summary>
    public class HistoricalEvent
    {
        /// <summary>
        /// Guid ID of the event.
        /// </summary>
        [Required]
        public virtual string Id { get; set; }

        [Required]
        public virtual DateTimeOffset TimestampUtc { get; set; }

        /// <summary>
        /// The assembly qualified name of the type.
        /// </summary>
        [Required]
        public virtual string EventType { get; set; }

        /// <summary>
        /// The JSON-serialized content of the event.
        /// </summary>
        [Required]
        public virtual string ContentJson { get; set; }
    }

    public class CaseClosedEvent : EventBase
    {
        public string CaseId { get; set; }
    }
}
