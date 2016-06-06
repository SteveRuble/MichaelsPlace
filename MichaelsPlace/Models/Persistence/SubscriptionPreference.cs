namespace MichaelsPlace.Models.Persistence
{
    public class SubscriptionPreference : UserPreference
    {
        public virtual string SubscriptionName { get; set; }
        public virtual bool IsEmailRequested { get; set; }
        public virtual bool IsSmsRequested { get; set; }
    }
}