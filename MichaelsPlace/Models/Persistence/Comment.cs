using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Comment : Notification
    {
        [Required]
        public virtual CaseItem CaseItem { get; set; }
    }
}