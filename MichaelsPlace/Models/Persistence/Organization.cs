using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Organization
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual Address Address { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public virtual string PhoneNumber { get; set; }

        [DisplayName("Fax Number")]
        [DataType(DataType.PhoneNumber)]
        public virtual string FaxNumber { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Notes { get; set; }

        private ICollection<Person> _people;

        private ICollection<Situation> _situations;

        public virtual ICollection<Situation> Situations
        {
            get { return _situations ?? (_situations = new HashSet<Situation>()); }
            set { _situations = value; }
        }

        public virtual ICollection<Person> People
        {
            get { return _people ?? (_people = new HashSet<Person>()); }
            set { _people = value; }
        }

        private ICollection<Case> _cases;

        public virtual ICollection<Case> Cases
        {
            get { return _cases ?? (_cases = new HashSet<Case>()); }
            set { _cases = value; }
        }



    }
}