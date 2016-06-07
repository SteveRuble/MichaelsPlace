using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class Invitation : EmailNotification
    {
        [Required]
        public virtual Case Case { get; set; }

        public virtual Person Invitee { get; set; }
    }
}