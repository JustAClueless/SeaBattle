using System;
using System.Windows.Forms;
using System.Drawing;

namespace Sea_Battle
{
    public class Output
    {
        private DataGridView grid1, grid2;
        private TextBox l;
        private Label result;

        public Output(DataGridView d1, DataGridView d2, TextBox l1, Label _result)
        {
            grid1 = d1;
            grid2 = d2;
            l = l1;
            result = _result;
        }

        public void setCellFor1(int i, int j, String s)
        {
            grid1[i, j].Value = s;
        }

        public void setCellFor2(int i, int j, String s)
        {
            grid2[i, j].Value = s;
        }

        public void setColor1(int i, int j, Color c)
        {
            grid1[i, j].Style.BackColor = c;
        }

        public void setColor2(int i, int j, Color c)
        {
            grid2[i, j].Style.BackColor = c;
        }

        public void setText(String s)
        {
            l.Text = s;
        }

        public void clearSelection()
        {
            grid1.ClearSelection();
            grid2.ClearSelection();
        }

        public void addText(String s)
        {
            l.Text += s;
        }

        public void clear(Color c)
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    grid1[i, j].Style.BackColor = c;
                    grid2[i, j].Style.BackColor = c;
                }
        }

        public void setResult(String s)
        {
            result.Text = s;
        }
    }
}
