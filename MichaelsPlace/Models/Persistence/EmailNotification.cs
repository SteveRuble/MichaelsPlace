using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class EmailNotification : Notification
    {
        [Required]
        public virtual string ToAddress { get; set; }

        [Required]
        public virtual string Subject { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, ToAddress: {ToAddress}, Subject: {Subject}";
        }
    }

    public class SmsNotification : Notification
    {
        [Required]
        public virtual string ToPhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, ToPhoneNumber: {ToPhoneNumber}";
        }
    }
}