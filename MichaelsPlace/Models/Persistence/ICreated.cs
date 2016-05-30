using System;

namespace MichaelsPlace.Models.Persistence
{
    public interface ICreated
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedUtc { get; set; }
    }
}