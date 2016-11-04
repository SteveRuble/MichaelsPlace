using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class CaseViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<PersonViewModel> CaseUsers { get; set; }
        public List<ItemViewModel> Articles { get; set; }
        public List<ItemViewModel> Todos { get; set; }
        public int? OrganizationId { get; set; }
    }
}