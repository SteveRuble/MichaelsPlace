using System;
using System.Collections.Generic;
using System.Linq;
using MichaelsPlace.Subscriptions;
using MichaelsPlace.Utilities;

namespace MichaelsPlace.Services
{
    /// <summary>
    /// Provides reflection-based information.
    /// </summary>
    public class ReflectionService
    {
        private readonly List<SubscriptionDescriptionAttribute> _subscriptionDescriptions;

        public ReflectionService()
        {
            _subscriptionDescriptions = GetType().Assembly.GetTypes().Where(t => ReflectionUtils.HasAttribute<SubscriptionDescriptionAttribute>(t))
                                                .SelectMany(t => Attribute.GetCustomAttributes(t, typeof(SubscriptionDescriptionAttribute)))
                                                .OfType<SubscriptionDescriptionAttribute>()
                                                .ToList();
        }
        
        public virtual IEnumerable<SubscriptionDescriptionAttribute> GetSubscriptionDescriptions()
        {
            return _subscriptionDescriptions;
        }
    }
}
