using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("First Name")]
        public string LastName { get; set; }

        [DisplayName("Disabled")]
        public bool IsDisabled { get; set; }

        public string EmailAddress { get; set; }

        [DisplayName("Locked Out")]
        public bool IsLockedOut { get; set; }
    }
}
