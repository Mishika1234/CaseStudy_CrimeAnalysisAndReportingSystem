using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CrimeAnalysisAndReportingSystem.util
{

    //Microsoft.Extensions.Configuration
    //Microsoft.Extensions.Configuration.FileExtension
    //Microst.extensions.Configuration.json
    internal static class DbConnUtil
    {
        private static IConfiguration _iconfiguration;
        //create a constructor
        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();

        }

        public static string GetConnection()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }
    }
}
