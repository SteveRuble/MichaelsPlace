using System;

namespace MichaelsPlace.Subscriptions
{
    /// <summary>
    /// Attribute which marks a listener as fulfilling subscriptions of the kind
    /// described by this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SubscriptionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// The name of the subscription.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of the subscription for UI presentation.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Attribute which marks a listener as fulfilling subscriptions of the kind
        /// described by this attribute.
        /// </summary>
        /// <param name="name">The name of the subscription. Should be a constant defined in <see cref="Constants.Subscriptions"/>.</param>
        /// <param name="description">The description of the subscription, displayed in the UI.</param>
        public SubscriptionDescriptionAttribute(string name, string description)
        {
            Description = description;
            Name = name;
        }
    }
}