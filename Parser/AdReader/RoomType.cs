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
        public override string ToString()
        {
            return Value;
        }
    }
}
