using System;

namespace MichaelsPlace.Infrastructure
{
    public interface IDurableEvent
    {
        Guid Id { get; }
    }
}