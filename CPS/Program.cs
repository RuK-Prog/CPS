using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace CPS
{
    public class INIManager
    {
        public INIManager(string aPath)
        {
            path = aPath;
        }

        public INIManager() : this("") { }

        public string GetPrivateString(string aSection, string aKey)
        {
            StringBuilder buffer = new StringBuilder(SIZE);
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);
            return buffer.ToString();
        }

        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            WritePrivateString(aSection, aKey, aValue, path);
        }

        public string Path { get { return path; } set { path = value; } }

        private const int SIZE = 1024; 
        private string path = null; 

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            bool ch = false, pMAX = false, pMIN = false;
            double pr = 1.00, prMAX = 0.79, prMIN = 0.20;
            int h = 0;
            INIManager manager = new INIManager(Directory.GetCurrentDirectory() + @"\settings.ini");
            /*
            manager.WritePrivateString("%", "MAX", prMAX.ToString());
            manager.WritePrivateString("%", "MIN", prMIN.ToString());
            manager.WritePrivateString("Sound", "MAX", "max");
            manager.WritePrivateString("Sound", "MIN", "max");
            manager.WritePrivateString("Key", "EXIT", "0");
            manager.WritePrivateString("Key", "REFRESH", "0");
            manager.WritePrivateString("Key", "AUTOSWITCH", "0");
            */
            if (!double.TryParse(manager.GetPrivateString("%", "MAX"), out prMAX) || prMAX > 1.00)
            {
                prMAX = 0.79;
            }
            if (!double.TryParse(manager.GetPrivateString("%", "MIN"), out prMIN))
            {
                prMIN = 0.20;
            }
            string pMAXp = Directory.GetCurrentDirectory() + @"\sound\" + manager.GetPrivateString("Sound", "MAX") + ".wav";
            string pMINp = Directory.GetCurrentDirectory() + @"\sound\" + manager.GetPrivateString("Sound", "MIN") + ".wav";
            SoundPlayer playerMAX = new SoundPlayer(pMAXp);
            SoundPlayer playerMIN = new SoundPlayer(pMINp);
            while (true)
            {
                if (manager.GetPrivateString("Key", "EXIT") == "1")
                {
                    if (manager.GetPrivateString("Key", "AUTOSWITCH") == "1")
                    {
                        manager.WritePrivateString("Key", "EXIT", "0");
                    }
                    break;
                }
                if (manager.GetPrivateString("Key", "REFRESH") == "1")
                {
                    h += 1;
                    if (manager.GetPrivateString("Key", "AUTOSWITCH") == "1")
                    {
                        manager.WritePrivateString("Key", "REFRESH", "0");
                        h += 1800;
                    }
                    if (h >= 1800)
                    {
                        h = 0; 
                        if (!double.TryParse(manager.GetPrivateString("%", "MAX"), out prMAX) || prMAX > 1.00)
                        {
                            prMAX = 0.79;
                        }
                        if (!double.TryParse(manager.GetPrivateString("%", "MIN"), out prMIN))
                        {
                            prMIN = 0.20;
                        }
                        playerMIN.Stop();
                        pMIN = false;
                        playerMAX.Stop();
                        pMAX = false;
                        pMAXp = Directory.GetCurrentDirectory() + @"\sound\" + manager.GetPrivateString("Sound", "MAX") + ".wav";
                        pMINp = Directory.GetCurrentDirectory() + @"\sound\" + manager.GetPrivateString("Sound", "MIN") + ".wav";
                        playerMAX = new SoundPlayer(pMAXp);
                        playerMIN = new SoundPlayer(pMINp);
                    }
                }
                PowerStatus pwr = SystemInformation.PowerStatus;
                switch (pwr.PowerLineStatus)
                {
                    case (PowerLineStatus.Offline):
                            ch = false;
                            break;
                    case (PowerLineStatus.Online):
                            ch = true;
                            break;
                    default:
                            break;
                }
                pr = pwr.BatteryLifePercent;
                if (ch && pr >= prMAX)
                {
                    if (!pMAX)
                    {
                        playerMAX.PlayLooping();
                        pMAX = true;
                    }
                }
                else if (!ch && pr >= prMAX-0.05)
                {
                    if (pMAX)
                    {
                        playerMAX.Stop();
                        pMAX = false;
                    }
                }
                else if (!ch && pr <= prMIN)
                {
                    if (!pMIN)
                    {
                        playerMIN.PlayLooping();
                        pMIN = true;
                    }
                }
                else if (ch && pr <= prMIN+0.05)
                {
                    if (pMIN)
                    {
                        playerMIN.Stop();
                        pMIN = false;
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}