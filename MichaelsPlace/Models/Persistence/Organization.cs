using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Organization
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual Address Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public virtual string PhaneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public virtual string FaxNumber { get; set; }

        public virtual string Notes { get; set; }

        private ICollection<ApplicationUser> _users;

        private ICollection<Situation> _situations;

        public virtual ICollection<Situation> Situations
        {
            get { return _situations ?? (_situations = new HashSet<Situation>()); }
            set { _situations = value; }
        }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return _users ?? (_users = new HashSet<ApplicationUser>()); }
            set { _users = value; }
        }

        private ICollection<Case> _cases;

        public virtual ICollection<Case> Cases
        {
            get { return _cases ?? (_cases = new HashSet<Case>()); }
            set { _cases = value; }
        }



    }
}