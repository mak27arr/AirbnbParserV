using AirbnbParser.Settings.Class;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatBot.Settings.Class
{
    [Serializable]
    public class Setting
    {
        [DataMember]
        public List<BaseConfiguration> BaseConfiguration = new List<BaseConfiguration>();
        [DataMember]
        public bool useCache = true;
        [DataMember]
        public int cacheUpdateInterval = 3600;
        [DataMember]
        public string ferioLogin;
        [DataMember]
        public string ferioPasswod;

    }
}
