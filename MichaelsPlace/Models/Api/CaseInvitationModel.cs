using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class CaseInvitationModel
    {
        public List<string> Addresses { get; set; }
        public string CaseId { get; set; }
    }
}