using ChatBot.Settings.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Settings.Interfaces
{
    interface ISettingsManager
    {
        public Setting Setting { get; set; }
        public bool LoadSettings();
        public bool SaveSattings();
    }
}
