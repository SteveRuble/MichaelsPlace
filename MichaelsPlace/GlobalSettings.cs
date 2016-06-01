using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MichaelsPlace
{
    /// <summary>
    /// Contains settings used throughout the application.
    /// </summary>
    public static partial class GlobalSettings
    {
        public static readonly JsonSerializerSettings EventSerializerSettings = new JsonSerializerSettings()
                                                                                {
                                                                                    TypeNameHandling = TypeNameHandling.All
                                                                                };

        static GlobalSettings()
        {
            Environment = ConfigurationManager.AppSettings["Environment"];
            IsDevelopment = Environment == "Dev";
            InitializeSecrets();
        }

        static partial void InitializeSecrets();

        public static string Environment { get; set; }

        public static bool IsDevelopment { get; set; }

        public static class Twilio
        {
            public static string AccountId { get; set; }
            public static string AuthToken { get; set; }
            public static string FromNumber { get; set; }

        }
    }
}
