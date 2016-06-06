using System;
using System.Collections.Generic;
using System.Linq;
using MichaelsPlace.Controllers.Admin;
using Ninject.Infrastructure.Language;
using MichaelsPlace.Utilities;

namespace MichaelsPlace.Subscriptions
{
    public class SubscriptionHelper
    {
        public SubscriptionHelper()
        {
            SubscriptionDescriptions = GetType().Assembly.GetTypes().Where(t => t.HasAttribute<SubscriptionDescriptionAttribute>())
                                                .SelectMany(t => Attribute.GetCustomAttributes(t, typeof(SubscriptionDescriptionAttribute)))
                                                .OfType<SubscriptionDescriptionAttribute>()
                                                .ToList();
        }

        public IEnumerable<SubscriptionDescriptionAttribute> SubscriptionDescriptions { get; private set; }
    }
}
