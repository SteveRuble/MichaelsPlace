namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    ///     Represents an <see cref="Item" />'s relationship to a case.
    /// </summary>
    public class CaseItem
    {
        public virtual int Id { get; set; }

        public virtual Case Case { get; set; }

        public virtual Item Item { get; set; }

        public virtual CaseItemStatus Status { get; set; }
    }
}