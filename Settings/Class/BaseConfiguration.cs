using ChatBot.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirbnbParser.Settings.Class
{
    public class BaseConfiguration
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool UseCredentical { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TableName { get; set; }
        public List<MyTuple<string,string>> AdictionalParamtr { get; set; }
        public string Certifacate { get; set; }
        public Type Conector { get; set; }
    } 
}
