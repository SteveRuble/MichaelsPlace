﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api.CaseDashboard
{
    public class PersonViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public bool CaseOwner { get; set; }
        public List<PersonItemViewModel> Articles { get; set; }
    }
}