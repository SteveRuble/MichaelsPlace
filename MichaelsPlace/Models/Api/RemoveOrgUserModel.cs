using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class RemoveOrgUserModel
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
    }
}