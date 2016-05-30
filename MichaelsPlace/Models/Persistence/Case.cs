using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MichaelsPlace.Models.Persistence
{
    public class Case : ICreated
    {
        private ICollection<CaseItem> _caseItems;

        private ICollection<CaseUser> _caseUsers;
        private string _id;

        [HiddenInput]
        public virtual string Id
        {
            get { return _id ?? (_id = Guid.NewGuid().ToString("N")); }
            set { _id = value; }
        }

        public virtual string Title { get; set; }

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

        private ICollection<CaseNote> _notes;

        public virtual ICollection<CaseNote> Notes
        {
            get { return _notes ?? (_notes = new HashSet<CaseNote>()); }
            set { _notes = value; }
        }

        [Required]
        [ReadOnly(true)]
        public virtual string CreatedBy { get; set; }

        [Required]
        [ReadOnly(true)]
        public virtual DateTimeOffset CreatedUtc { get; set; }
    }
}