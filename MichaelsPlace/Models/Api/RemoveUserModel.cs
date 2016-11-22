using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class RemoveUserModel
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
    }
}