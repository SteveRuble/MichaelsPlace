using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Organization : ISoftDelete
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ContextTag Context { get; set; }
        
        public virtual Address Address { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public virtual string PhoneNumber { get; set; }

        [DisplayName("Fax Number")]
        [DataType(DataType.PhoneNumber)]
        public virtual string FaxNumber { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Notes { get; set; }

        private ICollection<OrganizationPerson> _organizationPeople;

        public virtual ICollection<OrganizationPerson> OrganizationPeople
        {
            get { return _organizationPeople ?? (_organizationPeople = new HashSet<OrganizationPerson>()); }
            set { _organizationPeople = value; }
        }

        private ICollection<Case> _cases;

        public virtual ICollection<Case> Cases
        {
            get { return _cases ?? (_cases = new HashSet<Case>()); }
            set { _cases = value; }
        }

        public virtual bool IsDeleted { get; set; }
    }

    public class OrganizationPerson
    {
        public virtual int Id { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Person Person { get; set; }
        public virtual RelationshipTag Relationship { get; set; }
        public virtual bool IsOwner { get; set; }
    }
}