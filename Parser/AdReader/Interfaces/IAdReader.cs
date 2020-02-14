using AirbnbParser.Parser.AdReader.Class;
using AirbnbParser.Settings.Class;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirbnbParser.Parser.AdReader.Interfaces
{
    interface IAdReader
    {
        public List<Ad> GetAds();
        public bool Init(BaseConfiguration bc);
    }
}
