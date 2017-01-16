using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api.OrganizationDashboard
{
    public class OrganizationCaseViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
    }
}