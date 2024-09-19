using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace carregistersystem
{
   public class IniFile
    {
 


    

        
            public string Path;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

            public IniFile(string path)
            {
                Path = new FileInfo(path).FullName.ToString();
            }

            public string Read(string section, string key)
            {
                var retVal = new StringBuilder(255);
                int bytesRead = GetPrivateProfileString(section, key, "", retVal, 255, Path);




                return retVal.ToString();
            }
            public void Write(string Key, string Value, string Section = null)
            {
                WritePrivateProfileString(Section, Key, Value, Path);
            }

            public void UpdateProperty(string key, string newValue, string section = null)
            {
                Write(key, newValue, section);
            }
        
    }


}

