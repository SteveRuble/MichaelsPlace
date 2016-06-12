using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Models.Admin
{
    public class PersonModel
    {
        public string Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Disabled")]
        public bool IsDisabled { get; set; }

        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Locked Out")]
        public bool IsLockedOut { get; set; }

        [DisplayName("On Staff")]
        public bool IsStaff { get; set; }

        [DisplayName("Has Login")]
        [ReadOnly(true)]
        public bool HasApplicationUser { get; set; }

        [DisplayName("Email Confirmed")]
        [ReadOnly(true)]
        public bool IsEmailConfirmed { get; set; }

        [DisplayName("Phone Number Confirmed")]
        [ReadOnly(true)]
        public bool IsPhoneNumberConfirmed { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
    
}
