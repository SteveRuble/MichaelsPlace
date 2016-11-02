using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MichaelsPlace.Models.Api
{
    public class NewCaseModel
    {
        public SituationModel Situation { get; set; }
        public string Title { get; set; }
    }
}