using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carregistersystem
{
    public static class ConfigConnection
    {
        // Konfiguracijske postavke baze podataka
        public static string provider { get; private set; }
        public static string username { get; private set; }
        public static string password { get; private set; }
        public static string database { get; private set; }
        public static string db { get; private set; }



        public static void Initialize()
        {
            
      
            // Učitavanje konfiguracije baze podataka
            IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
            provider = ini.Read("DatabaseConfig", "Server");
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            database = ini.Read("DatabaseConfig", "dbupiti");
            db = ini.Read("DatabaseConfig", "DataBase");

        }
    }








}
