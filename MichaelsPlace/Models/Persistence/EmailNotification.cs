using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class EmailNotification : Notification
    {
        [Required]
        public virtual string ToAddress { get; set; }

        [Required]
        public virtual string Subject { get; set; }
    }

    public class SmsNotification : Notification
    {
        [Required]
        public virtual string ToPhoneNumber { get; set; }
    }
}