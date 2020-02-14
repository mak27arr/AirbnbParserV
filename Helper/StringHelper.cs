using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChatBot.Helper
{
    public static class StringHelper
    {
        public static Stream ToStream(this string value) => ToStream(value, Encoding.UTF8);

        public static Stream ToStream(this string value, Encoding encoding)
                                  => new MemoryStream(encoding.GetBytes(value ?? string.Empty));
    }
}
