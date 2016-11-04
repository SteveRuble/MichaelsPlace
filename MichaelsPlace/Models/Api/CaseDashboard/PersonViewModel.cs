using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class PersonViewModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<PersonItemViewModel> PersonItems { get; set; }
    }
}