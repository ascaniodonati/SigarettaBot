using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SigarettaBot
{
    internal static class Settings
    {
        //Impostazioni del bot
        public static string API_TOKEN = "2075040018:AAHEncRh5o66CxeWE6o0X_ULPtaVpcLQxfo";

        //Impostazioni del database
        const string DbHost = "localhost";
        const string DbUsername = "sigaretta";
        const string DbSchema = "sigaretta";
        const string DbPassword = "Ssigaretta.98s";
        public static string DbConnectionString = $"server={DbHost};database={DbSchema};user={DbUsername};password={DbPassword}";

        //Impostazioni del gioco
        public static List<string> DEFAULT_PHRASES
        {
            get
            {
                return new List<string>()
                {
                    "Chi è lui?",
                    "Chi è lei?",
                    "Cosa stanno facendo?",
                    "Dove?",
                    "Quando?",
                    "Cosa ha detto lui?",
                    "Cosa ha detto lei?",
                    "Cosa pensa la gente di loro??",
                };
            }
        }
    }
}
