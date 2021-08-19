using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using winFormAiGuess2;

namespace WinFormAiGuess2
{
    public partial class Form1 : Form
    {
            int[] allCombination = getAllCombination();
            int[] allGuess = new int[10];
            Tuple<int, int>[] responseToGuess = new Tuple<int, int>[10];
            ResponsSquare blackRespon,  whiteRespon;
            int indexOfGuess = 0;
            Color[] allColorInGame = new Color[] { Color.Red, Color.Blue, Color.Yellow,
                Color.Orange, Color.FromArgb(0,255 , 255), Color.Green };

        public void showSquare(int combToShow)
        {

            int[] arr = Int_to_array(combToShow);
            for (int i = 0; i < arr.Length; i++)
                new Square(Height / 10, allColorInGame[arr[i]-1],
                    (i+1) * Width / 8, Height / 8 * (indexOfGuess + 1)).show(this);
            
        }
        public Form1()
        {
            InitializeComponent();
          
            for (int i = 0; i < 6; i++)
            {
                new Square(Height / 15, allColorInGame[i],
                     (i + 1) * Width / 8, Height/100).show(this);
                     
            }
            Button btnOk = new Button()
            {
                Location = new Point(5 * Width / 8 + 4 * Width / 15, Height / 8 + Height / 50),
                Size = new Size(Height / 13, Height / 13),
                BackColor = Color.Ivory,
                Text = "OK",
                Font = new Font("", 12)
            };

            btnOk.Click += new System.EventHandler(confirm);
            Controls.Add(btnOk);

            allGuess[0] = allCombination[0];
            showSquare(allGuess[0]);
            createResponseSquare();
        }

        private void confirm(object sender, EventArgs e)
        {
            int sum = blackRespon.Amount + whiteRespon.Amount;
            if (sum >= 2 && sum <= 4)
            {
                responseToGuess[indexOfGuess] = new Tuple<int, int>(blackRespon.Amount, whiteRespon.Amount);

                if (responseToGuess[indexOfGuess].Item1 != 4)
                {
                    indexOfGuess++;
                    int i = 0;
                    bool isFoundCombin = false;
                    for (; !isFoundCombin && i < allCombination.Length; i++)
                    {
                        if (IsMatchCombination(allCombination[i]))
                        {
                            allGuess[indexOfGuess] = allCombination[i];
                            isFoundCombin = true;
                            showSquare(allGuess[indexOfGuess]);
                            createResponseSquare();
                        }
                    }
                    if (!isFoundCombin)
                        MessageBox.Show("Can't find match",
           "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                else
                {
                    string tempOut = "I was able to guess in " + (indexOfGuess+1)+" guesses!!!";
                    MessageBox.Show(tempOut,
             "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else MessageBox.Show("Invalid black and white pin input - please try again!",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public  void createResponseSquare()
        {

             blackRespon = new ResponsSquare(Height / 15, Color.Black,
                    6 * Width / 8, Height * (indexOfGuess + 1) / 8 + Height / 50,Color.White);

             whiteRespon = new ResponsSquare(Height / 15, Color.White,
                   6 * Width / 8 + 1 * Width / 15, Height * (indexOfGuess + 1) / 8 + Height / 50, Color.Black);
            
           
            blackRespon.show(this);
            whiteRespon.show(this);



        }
        static Tuple<int, int> compareTwoGuesses(int num1, int num2)
        {
            int correctLocationB = 0, notCorrectLocationW = 0;
            int[] arr1 = Int_to_array(num1);
            int[] arr2 = Int_to_array(num2);

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] == arr2[i])
                    correctLocationB++;
                else if (Array.Exists(arr2, element => element == arr1[i]))
                    notCorrectLocationW++;
                }

            return new Tuple<int,int>(correctLocationB,notCorrectLocationW);
        }
        static int[] Int_to_array(int n)
        {
            int j = 0;
            int len = n.ToString().Length;
            int[] arr = new int[len];
            while (n != 0)
            {
                arr[len - j - 1] = n % 10;
                n /= 10;
                j++;
            }
            return arr;
        }

        static int[] getAllCombination() {
            List<int> tempList = new List<int>();
            for (int i = 1234; i <= 6543; i++)
                if (existInGame(i))
                    tempList.Add(i);

            return  tempList.ToArray();
        }

        private static bool existInGame(int num)
        {
            ArrayList listOfDigitExist = new ArrayList() {1,2,3,4,5,6};

            while (num > 0)
            {
                listOfDigitExist.Remove(num % 10);
                num/= 10;
            }

            return listOfDigitExist.Count == 2;
        }

        public bool IsMatchCombination(int currentComb)
        {

            for (int i = 0; allGuess[i] != 0 && i < allGuess.Length; i++)
            {
                if (!compareTwoGuesses(allGuess[i], currentComb).Equals(responseToGuess[i]))
                    return false;
            }

            return true;
        }
    }
}
