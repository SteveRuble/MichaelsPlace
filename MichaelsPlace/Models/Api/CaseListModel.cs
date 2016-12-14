using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class CaseListModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int? OrganizationId { get; set; }
        public bool IsClosed { get; set; }
    }
}