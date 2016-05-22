using System;

namespace MichaelsPlace.Models.Persistence
{
    public interface ICreated
    {
        ApplicationUser CreatedBy { get; set; }
        DateTimeOffset CreatedUtc { get; set; }
    }
}