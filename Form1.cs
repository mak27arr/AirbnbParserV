using AirbnbParser.Parser.AdReader;
using AirbnbParser.Parser.AdReader.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirbnbParser
{
    public partial class Form1 : Form
    {

        private string savePatch = "";

        public Form1()
        {
            InitializeComponent();

            var room_type = RoomType.GetAllTypeList();
            comboBox2RoomType.DataSource = room_type;
            comboBox2RoomType.DisplayMember = "Item1";
            comboBox2RoomType.ValueMember = "Item2";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Filter = "Text File | *.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                savePatch = saveFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (savePatch.Length > 0)
            {
                var parser = new AirbnbReader();
                List<RoomType> rom_types = new List<RoomType>();
                if (comboBox2RoomType.SelectedItem != null)
                {
                    rom_types.Add(RoomType.SetValue((string)comboBox2RoomType.SelectedValue));
                }
                var rezalt = parser.GetAds((Tuple<string, string>)comboBox1.SelectedItem, dateTimePicker1.Value, dateTimePicker2.Value, Double.Parse(textBox1.Text.Replace(",",".")), Double.Parse(textBox2.Text.Replace(",", ".")), rom_types);
                progressBar1.Value = 0;
                progressBar1.Maximum = rezalt.Count;

                using (StreamWriter writetext = new StreamWriter(savePatch))
                {
                    foreach(var itm in rezalt)
                    {
                        writetext.WriteLine(itm.ToString());
                        progressBar1.Value++;
                    }
                }

                savePatch = "";
            }
            else
            {
                MessageBox.Show("Виберіть путь сохранения!!!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.Text.Length >= 3)
            {
                var parser = new AirbnbReader();
                var location = parser.GetLocation(comboBox1.Text);
                comboBox1.DataSource = location;
                comboBox1.DisplayMember = "Item2";
                comboBox1.ValueMember = "Item1";
            }
        }

    }
}
