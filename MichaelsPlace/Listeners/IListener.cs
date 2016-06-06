using System;
using MichaelsPlace.Infrastructure;

namespace MichaelsPlace.Handlers
{
    /// <summary>
    /// Contract to listen for events.
    /// </summary>
    public interface IListener
    {
        IDisposable SubscribeTo(IMessageBus bus);
    }
}