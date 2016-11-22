using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.OrganizationDashboard
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public AddressViewModel Address { get; set; }
        public List<OrganizationPersonViewModel> People { get; set; }
        public List<OrganizationCaseViewModel> Cases { get; set; }
    }
}