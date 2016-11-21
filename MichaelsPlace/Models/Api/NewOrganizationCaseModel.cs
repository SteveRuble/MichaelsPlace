using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class NewOrganizationCaseModel
    {
        public SituationModel Situation { get; set; }
        public string Title { get; set; }
        public int OrganizationId { get; set; }
    }
}