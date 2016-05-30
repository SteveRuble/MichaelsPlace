namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    /// Identifies event types which can have subscription preferences configured.
    /// </summary>
    public enum SubscribableEventType
    {
        Unknown,
        NewIndividualCase,
        NewOrganizationCase,
        VideoCallRequested,
        CaseClosed,
        PlanReviewNeeded

    }
}