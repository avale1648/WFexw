using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WFexw
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
            ReadStatistic();
        }
        private void ReadStatistic()
        {
            StreamReader sr = new StreamReader("stat.txt");
            string line = sr.ReadLine();
            while(line!=null)
            {
                listBox1.Items.Add(line);
                line = sr.ReadLine();
            }

        }
    }
    
}
