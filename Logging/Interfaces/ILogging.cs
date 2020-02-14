using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Logging
{
    interface ILogging
    {
        public void Msg(string msg, Object obj = null);
    }
}
