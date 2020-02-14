using AirbnbParser.Parser.AdReader.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirbnbParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            var t = new AirbnbReader();
            var ttt = t.GetLocation("Ternopil");
            InitializeComponent();
        }

    }
}
