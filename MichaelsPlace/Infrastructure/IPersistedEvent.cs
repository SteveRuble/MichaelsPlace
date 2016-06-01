using System;
using Newtonsoft.Json;

namespace MichaelsPlace.Infrastructure
{
    public abstract class EventBase
    {
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [JsonIgnore]
        public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.UtcNow;
    }
}