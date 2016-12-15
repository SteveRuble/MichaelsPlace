using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class RemoveCaseUserModel
    {
        public int UserId { get; set; }
        public string CaseId { get; set; }
    }
}