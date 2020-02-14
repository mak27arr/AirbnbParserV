using ChatBot.Logging.Class;
using ChatBot.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ChatBot.Settings.Class
{
    class SettingsManager : ISettingsManager
    {
        private string setting_file_name = "settings.xml";

        private Setting setting;
        private static SettingsManager instance;
        private static object syncRoot = new object();
        private static object syncLoadSet = new object();

        public Setting Setting
        {
            get { return setting; }
            set { if (value != null)
                    setting = value;
            }
        }

        private SettingsManager()
        {
            License.LicenseCheak.Cheak();
            if (setting == null)
                LoadSettings();
        }

        public static SettingsManager getInstance()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new SettingsManager();
                }
            }
            return instance;
        }

        public bool LoadSettings()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Setting));
            string patch = get_setting_file_patch();
            lock (syncLoadSet)
            {
                if (System.IO.File.Exists(patch))
                {
                    using (FileStream fs = new FileStream(patch, FileMode.OpenOrCreate))
                    {
                        setting = (Setting)formatter.Deserialize(fs);
                        Log l = new Log();
                        l.Msg("Setting loaded"); 
                    }
                }
                else
                {
                    setting = new Setting();
                }
            }
            return true;
        }

        public bool SaveSattings()
        {

            XmlSerializer formatter = new XmlSerializer(typeof(Setting));
            lock (syncLoadSet)
            {
                using (FileStream fs = new FileStream(get_setting_file_patch(), FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, setting);
                    Log l = new Log();
                    l.Msg("Setting saved");
                }
            }
            return true;
        }

        private string get_setting_file_patch()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + setting_file_name;
        }

    }
}
