using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MichaelsPlace.Models.Persistence
{
    public class Case : ICreated
    {
        private ICollection<CaseItem> _caseItems;

        private ICollection<CaseUser> _caseUsers;
        private string _id;

        public virtual string Id
        {
            get { return _id ?? (_id = Guid.NewGuid().ToString("N")); }
            set { _id = value; }
        }

        public virtual ICollection<CaseUser> CaseUsers
        {
            get { return _caseUsers ?? (_caseUsers = new HashSet<CaseUser>()); }
            set { _caseUsers = value; }
        }

        public virtual ICollection<CaseItem> CaseItems
        {
            get { return _caseItems ?? (_caseItems = new HashSet<CaseItem>()); }
            set { _caseItems = value; }
        }

        [Required]
        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public virtual DateTimeOffset CreatedUtc { get; set; }
    }
}