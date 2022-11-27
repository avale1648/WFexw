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
    public partial class Sudoku : Form
    {
        private int selectedX = 0, selectedY = 0;
        private int lifes = 3;
        private Random random = new Random();
        private int hintsNumber = 40;
        private const int n = 3;
        private const int cellSize = 50;
        private int[,] map = new int[n * n, n * n];
        private Button[,] cells = new Button[n * n, n * n];
        private string[,] notes = new string[n * n, n * n];
        public Sudoku()
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
            for (int i = 0; i < hintsNumber; i++)
            {
                int x = random.Next(n * n);
                int y = random.Next(n * n);
                cells[x, y].Text = map[x, y].ToString();
                cells[x, y].Enabled = false;
            }
        }
        private void cells_Click(object sender, EventArgs e)
        {
            //Button pressedCell = sender as Button;
            //string cellText = pressedCell.Text;
            //if(string.IsNullOrEmpty(cellText))
            //{
            //    pressedCell.Text = "1";
            //}
            //else
            //{
            //    int value = int.Parse(cellText);
            //    value++;
            //    if (value == 10) value = 1;
            //    pressedCell.Text = value.ToString();
            //}
            Button selectedCell = sender as Button;
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    if (selectedCell.Location == cells[i, j].Location)
                    {
                        selectedX = i;
                        selectedY = j;
                    }
                }
            }
            labelPlayerName.Text = $"{selectedX} {selectedY}";
        }
        private void buttonRubber_Click(object sender, EventArgs e)
        {
            if (cells[selectedX, selectedY].Enabled)
            {
                cells[selectedX, selectedY].Text = string.Empty;
                cells[selectedX, selectedY].ForeColor = Color.Black;
            }
        }

        private void buttonN_Click(object sender, EventArgs e)
        {
            Button buttonN = sender as Button;
            if (!checkBoxPen.Checked)//если не заметки
            {
                cells[selectedX, selectedY].Text = buttonN.Text;
                if (cells[selectedX, selectedY].Text == map[selectedX, selectedY].ToString())
                {
                    cells[selectedX, selectedY].Enabled = false;
                }
                else if (cells[selectedX, selectedY].Text.Length == 1 && cells[selectedX, selectedY].Text != map[selectedX, selectedY].ToString())
                {
                    cells[selectedX, selectedY].ForeColor = Color.Red;
                    lifes--; labelLifes.Text = $"Lifes: {lifes}";
                    if (lifes == 0)
                    {
                        MessageBox.Show("You lose...");
                        Close();
                    }
                }
            }
            else//если заметки
            {
                if (cells[selectedX, selectedY].Enabled)
                {
                    cells[selectedX, selectedY].ForeColor = Color.DarkGray;
                    if (!cells[selectedX, selectedY].Text.Contains(buttonN.Text))
                        cells[selectedX, selectedY].Text += buttonN.Text;
                }
            }
        }
    }
}
