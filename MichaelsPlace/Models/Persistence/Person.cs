using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MichaelsPlace.Models.Persistence
{
    public class Person : ISoftDelete
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString("N");

        public virtual ApplicationUser ApplicationUser { get; set; }

        [DisplayName("First Name")]
        public virtual string FirstName { get; set; }

        [DisplayName("First Name")]
        public virtual string LastName { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string PhoneNumber { get; set; }

        private ICollection<PersonCaseItem> _caseItems;
        private ICollection<PersonCase> _cases;

        public virtual ICollection<PersonCase> PersonCases
        {
            get { return _cases ?? (_cases = new HashSet<PersonCase>()); }
            set { _cases = value; }
        }

        public virtual ICollection<PersonCaseItem> PersonCaseItems
        {
            get { return _caseItems ?? (_caseItems = new HashSet<PersonCaseItem>()); }
            set { _caseItems = value; }
        }

        public virtual bool IsDeleted { get; set; }
    }
}