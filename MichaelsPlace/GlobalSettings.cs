using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MichaelsPlace
{
    public enum RunningEnvironment
    {
        Dev,
        Prod
    }

    /// <summary>
    /// Contains settings used throughout the application.
    /// </summary>
    public static partial class GlobalSettings
    {
        /// <summary>
        /// JSON settings for events.
        /// </summary>
        public static readonly JsonSerializerSettings EventSerializerSettings = new JsonSerializerSettings()
                                                                                {
                                                                                    TypeNameHandling = TypeNameHandling.All
                                                                                };

        static GlobalSettings()
        {
            Environment = (RunningEnvironment)Enum.Parse(typeof(RunningEnvironment), ConfigurationManager.AppSettings["Environment"]);
            IsDevelopment = Environment == RunningEnvironment.Dev;
            InitializeSecrets();
        }

        static partial void InitializeSecrets();

        /// <summary>
        /// Gets the environment we're running in.
        /// </summary>
        public static RunningEnvironment Environment { get; set; }

        /// <summary>
        /// Will be true if we're in development mode.
        /// </summary>
        public static bool IsDevelopment { get; set; }

        /// <summary>
        /// Settings for Twilio SMS API.
        /// </summary>
        public static class Twilio
        {
            public static string AccountId { get; set; }
            public static string AuthToken { get; set; }
            public static string FromNumber { get; set; }

        }
    }
}
