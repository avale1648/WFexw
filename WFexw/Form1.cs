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
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private int hintsNumber = 40;
        private const int n = 3;
        private const int cellSize = 50;
        private int[,] map = new int[n * n, n * n];
        private Button[,] cells = new Button[n * n, n * n];
        public Form1()
        {
            InitializeComponent();
            GenerateMap();
        }
        public void GenerateMap()
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                    map[i, j] = (i * n + i / n + j) % (n * n) + 1;
            }
            for (int i = 0; i < 10; i++)
                MixMapOptionChoose(random.Next(5));
            CreateMap();
            ShowHints();
        }
        private void MatrixTransposition()
        {
            int[,] mapCopy = new int[n * n, n * n];
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    mapCopy[i, j] = map[i, j];
                }
            }
            map = mapCopy;
        }
        private void MixRowsInBlock()
        {
            int block = random.Next(n);
            int row1 = random.Next(n);
            int col1 = block * n + row1;
            int row2 = random.Next(n);
            while (row1 == row2)
                row2 = random.Next(n);
            int col2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                int temp = map[col1, i];
                map[col1, i] = map[col2, i];
                map[col2, i] = temp;
            }
        }
        private void MixColumnsInBlock()
        {
            int block = random.Next(n);
            int row1 = random.Next(n);
            int col1 = block * n + row1;
            int row2 = random.Next(n);
            while (row1 == row2)
                row2 = random.Next(n);
            int col2 = block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                int temp = map[i, col1];
                map[i, col1] = map[i, col2];
                map[i, col2] = temp;
            }
        }
        private void MixBlocksInRow()
        {
            int block1 = random.Next(n);
            int block2 = random.Next(n);
            while (block1 == block2)
                block2 = random.Next(n);
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                int k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    int temp = map[j, i];
                    map[j, i] = map[k, i];
                    map[k, i] = temp;
                    k++;
                }
            }
        }
        private void MixBlocksInColumn()
        {
            int block1 = random.Next(n);
            int block2 = random.Next(n);
            while (block1 == block2)
                block2 = random.Next(n);
            block1 *= n;
            block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                int k = block2;
                for (int j = block1; j < block1 + n; j++)
                {
                    int temp = map[i, j];
                    map[i, j] = map[i, k];
                    map[i, k] = temp;
                    k++;
                }
            }
        }
        private void CreateMap()
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    cells[i, j] = new Button();
                    cells[i, j].Size = new Size(cellSize, cellSize);
                    cells[i, j].Location = new Point(i * cellSize, j * cellSize);
                    cells[i, j].FlatStyle = FlatStyle.Flat;
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? Color.LightBlue : Color.SkyBlue;
                    cells[i, j].Click += cells_Click;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        private void MixMapOptionChoose(int option)
        {
            switch (option)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    MixRowsInBlock();
                    break;
                case 2:
                    MixColumnsInBlock();
                    break;
                case 3:
                    MixBlocksInRow();
                    break;
                case 4:
                    MixBlocksInColumn();
                    break;
            }
        }
        private void ShowHints()
        {
            for(int i = 0; i < hintsNumber; i++)
            {
                int x = random.Next(n * n);
                int y = random.Next(n * n);
                cells[x, y].Text = map[x, y].ToString();
                cells[x, y].Enabled = false;
            }
        }
        private void cells_Click(object sender, EventArgs e)
        {
            Button pressedCell = sender as Button;
            string cellText = pressedCell.Text;
            if(string.IsNullOrEmpty(cellText))
            {
                pressedCell.Text = "1";
            }
            else
            {
                int value = int.Parse(cellText);
                value++;
                if (value == 10) value = 1;
                pressedCell.Text = value.ToString();
            }
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            int fails = 0;
            for(int i = 0; i < n*n; i++)
            {
                for(int j = 0; j < n*n;j++)
                {
                    string cellText = cells[i, j].Text;
                    if (cellText != map[i, j].ToString())
                        fails++;
                }
            }
            if(fails == 0)
                MessageBox.Show("Congradilations! You solved Sudoku without errors.", "Victory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show($"You lose, sudoku was solved with {fails} errors.", "Lose", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }
}
