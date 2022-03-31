using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormAiGuess2;

namespace winFormAiGuess2
{
    class Square
    // This class represents one square on the screen
    {
        private int ribLength;                   // the length of the rib of the square 
        private Color backColor;                 // color of square
        private int locationX, locationY;        // location of square on screen
        protected Label currentRectangle;        // label object that represents this square

        public Color Color { get => backColor; set { backColor = value; changeProperty(); } }
        public int LocationX { get => locationX; set { locationX = value; changeProperty(); } }
        public int LocationY { get => locationY; set { locationY = value; changeProperty(); } }
        public int RibLength { get => ribLength; set { ribLength = value; changeProperty(); } }

        public Square(int ribLength, Color color, int locationX, int locationY)
        {
            this.ribLength = ribLength;
            this.backColor = color;
            this.locationX = locationX;
            this.locationY = locationY;

            currentRectangle = new Label();
            changeProperty();
        }
        private void changeProperty()
        {
            currentRectangle.Location = new Point(locationX, locationY);
            currentRectangle.Size = new Size(ribLength, ribLength);
            currentRectangle.BackColor = backColor;
        }

        public void show(Form form)
        {
            // This method recive form to show the current square
            // and the method show the square with animation 

            currentRectangle.Width = 0;
            var timerAnim = new System.Windows.Forms.Timer();

            timerAnim.Interval = 30;
            timerAnim.Start();

            timerAnim.Tick += (_, args) =>
            {
                currentRectangle.Width += 1;
                if (currentRectangle.Width == ribLength)
                    timerAnim.Stop();
            };

            form.Controls.Add(currentRectangle);
        }

    }
}
