using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFexw
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int hints = 0;
            if (radioButton1.Checked)
                hints = 60;
            if (radioButton2.Checked)
                hints = 40;
            if (radioButton3.Checked)
                hints = 20;
            if (radioButton4.Checked)
                hints = 9;
            Sudoku sudoku = new Sudoku(textBox1.Text, hints);
            sudoku.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.Show();
        }
    }
}
