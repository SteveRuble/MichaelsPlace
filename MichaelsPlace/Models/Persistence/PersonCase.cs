using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    ///     Represents a user's relationship to a case.
    /// </summary>
    public class PersonCase
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual Case Case { get; set; }

        [Required]
        public virtual Person Person { get; set; }

        [Required]
        public virtual RelationshipTag Relationship { get; set; }
    }
}