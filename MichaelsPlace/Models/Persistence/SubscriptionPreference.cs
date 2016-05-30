namespace MichaelsPlace.Models.Persistence
{
    public class SubscriptionPreference : UserPreference
    {
        public virtual SubscribableEventType EventType { get; set; }
        public bool IsEmailRequested { get; set; }
        public bool IsSmsRequested { get; set; }
    }
}