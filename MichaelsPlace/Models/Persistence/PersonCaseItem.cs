namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    ///     Represents a <see cref="ApplicationUser" />'s relationship
    ///     to an item (i.e., if they have viewed it, been assigned to it, etc.)
    /// </summary>
    public class PersonCaseItem
    {
        public virtual int Id { get; set; }

        public virtual Case Case { get; set; }

        public virtual Item Item { get; set; }

        public virtual Person Person { get; set; }

        public virtual CaseItemUserStatus Status { get; set; }
    }
}