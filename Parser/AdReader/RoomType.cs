using System;
using System.Collections.Generic;
using System.Text;

namespace AirbnbParser.Parser.AdReader
{
    class RoomType
    {
        private RoomType(string value) { Value = value; }
        public string Value { get; set; }

        public static RoomType EntirehomeFapt { get { return new RoomType("Entire%20home%2Fapt"); } }
        public static RoomType Privateroom { get { return new RoomType("Private%20room"); } }
        public static RoomType Hotelroom { get { return new RoomType("Hotel%20room"); } }
        public static RoomType Sharedroom { get { return new RoomType("Shared%20room"); } }
        public static RoomType SetValue(string value)
        {
            return new RoomType(value);
        }
        public static List <Tuple<string,string>>GetAllTypeList()
        {
            List<Tuple<string, string>> type = new List<Tuple<string, string>>();
            type.Add(new Tuple<string, string>("EntirehomeFapt", "Entire%20home%2Fapt"));
            type.Add(new Tuple<string, string>("Privateroom", "Private%20room"));
            type.Add(new Tuple<string, string>("Hotelroom", "Hotel%20room"));
            type.Add(new Tuple<string, string>("Sharedroom", "Shared%20room"));
            return type;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
