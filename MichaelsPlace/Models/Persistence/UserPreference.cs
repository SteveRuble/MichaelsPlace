using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    /// <summary>
    /// Base class for user preferences.
    /// </summary>
    public class UserPreference
    {
        public virtual int Id { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}