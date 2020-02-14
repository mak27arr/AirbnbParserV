using ChatBot.Settings.Class;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using CefSharp;
using CefSharp.OffScreen;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using AirbnbParser.Parser.AdReader.Class;
using AirbnbParser.Parser.AdReader.Interfaces;
using AirbnbParser.Settings.Class;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace AirbnbParser.Parser.AdReader.Class
{
    class AirbnbReader : IAdReader
    {
        private string base_url = "https://www.airbnb.com.ua/s/homes";
        private string getCityUrl = "https://www.airbnb.com.ua/api/v2/autocompletes?country=UA&key=d306zoyjsyarp7ifhu67rjxn52tv0t20&language=EU&locale=uk&num_results=5&user_input={0}&api_version=1.2.6&vertical_refinement=homes&region=-1&options=should_show_stays";
        private string cityName = "https://www.airbnb.com.ua/s/{0}/homes?";
        private string FullUrl = "https://www.airbnb.com.ua/s/homes?refinement_paths%5B%5D=%2Fhomes&current_tab_id=home_tab&selected_tab_id=home_tab&screen_size=large&hide_dates_and_guests_filters=false&checkin=2020-03-01&checkout=2020-03-31&search_type=filter_change&room_types%5B%5D=Entire%20home%2Fapt&room_types%5B%5D=Private%20room&room_types%5B%5D=Hotel%20room&room_types%5B%5D=Shared%20room&price_min=205&price_max=13551";
        //{0} - place
        //{1} - price max ( &price_max={1} )
        //{2} - price min ( &price_min={2} )
        //{3} - place name
        //{4} - cheakin yyyy-mm-dd ( &checkin={4} )
        //{5} - cheakout ( &checkout={5} )

        //{6} - roomtype
        //&room_types%5B%5D=Entire%20home%2Fapt
        //&room_types%5B%5D=Private%20room
        //&room_types%5B%5D=Hotel%20room
        //&room_types%5B%5D=Shared%20room

        //{7} - per_page
        //{8} - offset &items_offset=20

        private string filterUrlApi = "https://www.airbnb.com.ua/api/v2/explore_tabs?_format=for_explore_search_web&auto_ib=false{4}{5}&client_session_id=c5dbb7c8-e03b-44e9-8e42-a89b069f93ae&currency=USD&current_tab_id=home_tab&experiences_per_grid={7}&fetch_filters=true&guidebooks_per_grid={7}&has_zero_guest_treatment=true&hide_dates_and_guests_filters=false&is_guided_search=true&is_new_cards_experiment=true&is_standard_search=true&items_per_grid={7}&key=d306zoyjsyarp7ifhu67rjxn52tv0t20&locale=uk&metadata_only=false&place_id={0}{1}{2}&query={3}&query_understanding_enabled=true&refinement_paths%5B%5D=%2Fhomes{6}&satori_version=1.2.6&screen_height=722&screen_size=medium&screen_width=906&search_type=filter_change&selected_tab_id=home_tab&show_groupings=true&supports_for_you_v3=true&timezone_offset=120&version=1.7.3{8}";
        private string roomUrl = "https://www.airbnb.com.ua/rooms/{0}";

        private object rezaltObj = null;
        private bool complitLoad = false;
        private ChromiumWebBrowser browser;

        public AirbnbReader()
        {
            this.Init();
        }

        public List<Ad> GetAds()
        {
            throw new NotImplementedException();
        }

        private bool Init()
        {
            //SettingsManager sm = SettingsManager.getInstance();
            return Login("", "");
        }

        private bool Login(string login,string password)
        {
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            browser = new ChromiumWebBrowser(base_url);
            browser.LoadingStateChanged += BrowserLoadingStateChanged;
            return true;
        }

        public List<Tuple<string,string>> GetLocation(string filter)
        {
            List<Tuple<string, string>> rezalt = new List<Tuple<string, string>>();
            complitLoad = false;
            var ready = false;
            browser.Load(string.Format(getCityUrl,filter));
            while (!complitLoad || !ready)
            {
                if (complitLoad)
                {
                    string rez_str = rezaltObj as string;
                    rez_str = Regex.Replace(rez_str, "<.*?>", String.Empty);
                    JObject jObject = JObject.Parse(rez_str);
                    foreach(var item in jObject["autocomplete_terms"])
                    {
                        rezalt.Add(new Tuple<string, string>(((string)item["id"]), ((string)item["display_name"])));
                    }
                    ready = true;
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
            return rezalt;
        }


        //{0} - place
        //{1} - price max ( &price_max={1} )
        //{2} - price min ( &price_min={2} )
        //{3} - place name
        //{4} - cheakin yyyy-mm-dd ( &checkin={4} )
        //{5} - cheakout ( &checkout={5} )

        //{6} - roomtype
        //&room_types%5B%5D=Entire%20home%2Fapt
        //&room_types%5B%5D=Private%20room
        //&room_types%5B%5D=Hotel%20room
        //&room_types%5B%5D=Shared%20room

        //{7} - per_page
        //{8} - offset &items_offset=20


        public List<Ad> GetAd(Tuple<string,string> place,DateTime cheakin, DateTime cheakout,double price_min,double price_max,List<RoomType> room_type)
        {
            List<Ad> rezalt = new List<Ad>();
            complitLoad = false;
            var ready = false;

            string url = string.Format(filterUrlApi);

            browser.Load(url);
            while (!complitLoad || !ready)
            {
                if (complitLoad)
                {
                    string rez_str = rezaltObj as string;
                    rez_str = Regex.Replace(rez_str, "<.*?>", String.Empty);
                    JObject jObject = JObject.Parse(rez_str);
                    foreach (var item_tab in jObject["explore_tabs"])
                    {
                        foreach (var item_sec in item_tab["section"])
                        {
                            foreach(var item in item_sec["listings"])
                            {
                                Ad ad_item = new Ad();
                                ad_item.url = string.Format(roomUrl,item["badget"]["id"]);
                                ad_item.country = (string)item["badget"]["localized_city"];
                                ad_item.data = "";
                                ad_item.price = (string)item["pricing_quote"]["rate"]["amount"];
                                ad_item.type = (string)item["badget"]["space_type"];
                                ad_item.feedbeack = (string)item["badget"]["reviews_count"];
                                ad_item.host = string.Format("users/show/{0}", item["badget"]["295371717"]);
                                rezalt.Add(ad_item);
                            }
                        }
                    }
                    ready = true;
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
            return rezalt;
        }

        private void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                browser.LoadingStateChanged -= BrowserLoadingStateChanged;
                SettingsManager sm = SettingsManager.getInstance();

                string script_add_jquery = "document.getElementsByTagName ('html')[0].innerHTML";
                var scriptTask = browser.EvaluateScriptAsync(script_add_jquery);

                scriptTask.ContinueWith(t =>
                {
                    rezaltObj = t.Result.Result;
                    complitLoad = true;
                });
            }
        }

        bool IAdReader.Init(BaseConfiguration bc)
        {
            throw new NotImplementedException();
        }
    }
}