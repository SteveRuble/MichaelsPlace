using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.OrganizationDashboard
{
    public class OrganizationPersonViewModel
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string DisplayName { get; set; }
    }
}