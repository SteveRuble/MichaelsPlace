using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MichaelsPlace
{
    public static class GlobalSettings
    {
        public static readonly JsonSerializerSettings EventSerializerSettings = new JsonSerializerSettings()
                                                                                {
                                                                                    TypeNameHandling = TypeNameHandling.All
                                                                                };

        static GlobalSettings()
        {
            Environment = ConfigurationManager.AppSettings["Environment"];
            IsDevelopment = Environment == "Dev";
        }

        public static string Environment { get; set; }

        public static bool IsDevelopment { get; set; }
    }
}
