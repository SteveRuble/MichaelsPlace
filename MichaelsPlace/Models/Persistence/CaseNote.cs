namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    /// A note added to a case by a Michael's Place admin.
    /// </summary>
    public class CaseNote : Notification
    {
        public virtual Case Case { get; set; }
    }
}