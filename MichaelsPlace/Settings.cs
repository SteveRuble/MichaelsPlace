using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace
{
    public static class GlobalSettings
    {
        static GlobalSettings()
        {
            Environment = ConfigurationManager.AppSettings["Environment"];
            IsDevelopment = Environment == "Dev";
        }

        public static string Environment { get; set; }

        public static bool IsDevelopment { get; set; }
    }
}
