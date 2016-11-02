using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api
{
    public class CaseViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<PersonCase> CaseUsers { get; set; }
        public ICollection<CaseItem> CaseItems { get; set; }
        public ICollection<CaseNote> Notes { get; set; }
        public Organization Organization { get; set; }
    }
}