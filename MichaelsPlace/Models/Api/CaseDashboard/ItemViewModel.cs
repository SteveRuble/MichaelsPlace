using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemTitle { get; set; }
        public CaseItemStatus Status { get; set; }
    }
}