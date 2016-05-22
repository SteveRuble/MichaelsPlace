using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Email : Notification
    {
        [Required]
        public virtual string ToAddress { get; set; }

        [Required]
        public virtual string Subject { get; set; }
    }
}