using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sea_Battle
{
    public partial class Form1 : Form
    {
        int x0 = -1, y0 = -1, x = -1, y = -1;
        Game game;
        bool gameIsOn;
        Output output;

        public Form1()
        {
            InitializeComponent();
            grid1.RowCount = grid1.ColumnCount;
            grid2.RowCount = grid1.ColumnCount;

            //подсказка
            dataGridView1.RowCount = 4;
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.ClearSelection();
            dataGridView1[0, 0].Style.BackColor = Color.White;
            dataGridView1[0, 1].Style.BackColor = Color.PowderBlue;
            dataGridView1[0, 2].Style.BackColor = Color.Blue;
            dataGridView1[0, 3].Style.BackColor = Color.Black;


            dataGridView1[1, 0].Value = "Пусто";
            dataGridView1[1, 1].Value = "Мимо";
            dataGridView1[1, 2].Value = "Корабль";
            dataGridView1[1, 3].Value = "Попадание";


            label5.Text = label6.Text = "а\r\nб\r\nв\r\nг\r\nд\r\nе\r\nж\r\nз\r\nи\r\nк";
            label7.Text = label8.Text = " 1   2   3   4   5   6   7   8   9  10";

            grid1.ClearSelection();
            grid2.ClearSelection();

            output = new Output(grid1, grid2, textBox1, labelResult);
            game = new Game(output);
            gameIsOn = false;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!gameIsOn)
            {
                x0 = e.ColumnIndex;
                y0 = e.RowIndex;
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            grid2.ClearSelection();
            if (!gameIsOn)
            {
                x = e.ColumnIndex;
                y = e.RowIndex;
                game.placePlayerShip(x0, y0, x, y);
                if (game.isReadyToPlay() && !gameIsOn)
                {
                    buttonClear.Visible = false;
                    buttonRandom.Visible = false;
                    gameIsOn = true;
                    game.start();
                    grid1.ReadOnly = true;
                }
            }
        }

        private void clear(object sender, EventArgs e)
        {
            game.clear();
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            grid2.ClearSelection();
            if (game.isEnd())
                gameIsOn = false;
            if (gameIsOn)
            {
                x = e.ColumnIndex;
                y = e.RowIndex;
                game.shot(x, y);
            }
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            startNew();
            game.randomP1();
            buttonClear.Visible = false;
            buttonRandom.Visible = false;
            gameIsOn = true;
            if (game.isEnd())
                gameIsOn = false;
        }


        private void startNew()
        {
            buttonClear.Visible = true;
            buttonRandom.Visible = true;
            game = new Game(output);
            //game.start();
            gameIsOn = false;
            labelResult.Text = "";
        }

        private void newGame(object sender, EventArgs e)
        {
            startNew();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellMouseUp_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа была разработана \r\nВасильевым Евгением Андреевичем, \r\nстундентом ОмГТУ, ФИТиКС,  группы ИВТ-172.\r\n2019 г.");
        }
    }
}
