using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk
{
    public static class InfinniSession
    {
        public static CookieContainer CookieContainer { get; set; }

        public static string Server { get; set; }

        public static string Port { get; set; }

        public static string Version { get; set; }

        public static bool Initialized
        {
            get
            {
                return !string.IsNullOrEmpty(Server) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Version);
            }
        }
    }
}
