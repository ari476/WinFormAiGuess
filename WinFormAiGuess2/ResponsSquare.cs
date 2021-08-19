using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winFormAiGuess2;

namespace WinFormAiGuess2
{
    class ResponsSquare : Square
    {
        private string text;
        private Color foreColor;
        private int amount;

        public ResponsSquare(int ribLength, Color color, int locationX, int locationY,
            Color foreColor) : base(ribLength, color, locationX, locationY)
        {
            this.Text = "0";
            this.ForeColor = foreColor;
            amount = 0;
            currentRectangle.Click += new System.EventHandler(OnClick);
        }

        private void OnClick(object sender, EventArgs e)
        {
            amount++;
            amount %= 5;
            Text = "" + amount;
        }

        public string Text { get => text; set { text = value; changeProperty(); } }
        public Color ForeColor { get => foreColor; set { foreColor = value; changeProperty(); } }

        public int Amount { get => amount;}

        private void changeProperty()
        {
            currentRectangle.Text = text;
            currentRectangle.ForeColor = foreColor;
            currentRectangle.Font = new Font("", 20);
            currentRectangle.TextAlign = ContentAlignment.MiddleCenter;
        }

    }
}
