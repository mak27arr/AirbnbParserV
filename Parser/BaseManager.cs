using ChatBot.Settings.Class;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AirbnbParser.Parser.AdReader.Class;
using AirbnbParser.Parser.AdReader.Interfaces;

namespace AirbnbParser.Parser
{
    class BaseManager
    {
        private Timer cacheTimer;
        private DateTime lastCacheUpdate;
        private object syncObj;

        private HashSet<Ad> carPartsCache = new HashSet<Ad>();

        public BaseManager()
        {
            Init();
        }

        private void Init()
        {
            SettingsManager sm = SettingsManager.getInstance();
            if (sm.Setting.useCache)
            {
                SetUpdateCache(sm.Setting.cacheUpdateInterval);
            }
            else
            {
                //do thamsing 
            }
        }

        private void SetUpdateCache(int interval)
        {
            cacheTimer = new Timer(UpdateCache, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(interval));
        }

        private void UpdateCache(object state)
        {
            carPartsCache = new HashSet<Ad>();
            lastCacheUpdate = DateTime.Now;
            SettingsManager sm = SettingsManager.getInstance();
            
            Parallel.ForEach(sm.Setting.BaseConfiguration,db=> {
                var conector  = Activator.CreateInstance(db.Conector) as IAdReader;
                if (conector != null)
                {
                    conector.Init(db);
                    var db_car_part = conector.GetAds();
                    lock (syncObj)
                    {
                        carPartsCache.UnionWith(db_car_part);
                    }
                }
            });
        }

        public List<Ad> FindCarPart(string brand="", string fullname="",int year=0,string partnumber="")
        {
            List<Ad> retValue = new List<Ad>();
            SettingsManager sm = SettingsManager.getInstance();
            if (! sm.Setting.useCache)
            {
                UpdateCache(null);
            }

            if (partnumber != "")
            {
                return null;
                //return carPartsCache.Where(e => e.PartName.Replace(" ","").ToLower() == partnumber.Replace(" ", "").ToLower()).ToList();
            }

            if (brand != "")
            {
                return null;
                //retValue = carPartsCache.Where(e=>e.CarBrand.Replace(" ","").ToLower().Contains(brand.Replace(" ", "").ToLower())).ToList();
            }

            if (retValue.Count > 0)
            {
                return retValue;
            }
            else
            {
                
            }

            if (!sm.Setting.useCache)
                carPartsCache.Clear();
            return retValue;
        }
    }
}
