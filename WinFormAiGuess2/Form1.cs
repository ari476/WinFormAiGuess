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
        // allCombination - Array taht include all the numbers in the game
        int[] allCombination = getAllCombination();
        // allGuess - All the guesses that the computer guessed
        int[] allGuess = new int[10];
        // responseToGuess - An array of pairs that contains all 
        //the user's responses according to the computer's guess
        Tuple<int, int>[] responseToGuess = new Tuple<int, int>[10];

        ResponsSquare blackRespon, whiteRespon;

        // indexOfGuess - The index of the current guess
        int indexOfGuess = 0;
        // i - The current index of number to check if 
        // the computer's guess is can be the next guess
        int i = 0;

        Color[] allColorInGame = new Color[] { Color.Red, Color.Blue, Color.Yellow,
                Color.Orange, Color.FromArgb(0,255 , 255), Color.Green };

      
        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 6; i++)
            {
                new Square(Height / 15, allColorInGame[i],
                     (i + 1) * Width / 8, Height / 100).show(this);

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
        public void showSquare(int combToShow)
        {
            // Gets a number that is a specific combination and displays it on the screen in 
            // color squares corresponding to each digit in the number 

            int[] arr = Int_to_array(combToShow);
            for (int i = 0; i < arr.Length; i++)
                new Square(Height / 10, allColorInGame[arr[i] - 1],
                    (i + 1) * Width / 8, Height / 8 * (indexOfGuess + 1)).show(this);

        }
        private void confirm(object sender, EventArgs e)
        {
            //This function is called when the user has confirmed
            //his response to the computer's last guess
            int sum = blackRespon.Amount + whiteRespon.Amount;
            if (sum >= 2 && sum <= 4)
            {
                responseToGuess[indexOfGuess] = new Tuple<int, int>(blackRespon.Amount, whiteRespon.Amount);

                if (responseToGuess[indexOfGuess].Item1 != 4)
                {
                    indexOfGuess++;
                    bool isFoundCombin = false;
                    // This for loop stops if he finds a suitable number to guess
                    // and the next time he guesses he starts from the same place he stopped (from i)
                    for (; i < allCombination.Length; i++)
                    {
                        if (IsMatchCombination(allCombination[i]))
                        {
                            allGuess[indexOfGuess] = allCombination[i];
                            isFoundCombin = true;
                            showSquare(allGuess[indexOfGuess]);
                            createResponseSquare();
                            break;
                        }
                    }
                    if (!isFoundCombin)
                        MessageBox.Show("Can't find match",
           "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                else
                {
                    string tempOut = "I was able to guess in " + (indexOfGuess + 1) + " guesses!!!";
                    MessageBox.Show(tempOut,
             "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else MessageBox.Show("Invalid black and white pin input - please try again!",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void createResponseSquare()
        {
            //This function produces the two squares of the following responses 
            //that the user will receive and displays them on the screen

            blackRespon = new ResponsSquare(Height / 15, Color.Black,
                   6 * Width / 8, Height * (indexOfGuess + 1) / 8 + Height / 50, Color.White);

            whiteRespon = new ResponsSquare(Height / 15, Color.White,
                  6 * Width / 8 + 1 * Width / 15, Height * (indexOfGuess + 1) / 8 + Height / 50, Color.Black);


            blackRespon.show(this);
            whiteRespon.show(this);



        }
        static Tuple<int, int> compareTwoGuesses(int num1, int num2)
        {
            // Gets 2 numbers and returns a pair representing the number of correct positions 
            // between them (black) and those in the wrong places (white)

            int correctLocationB = 0, notCorrectLocationW = 0;
            int[] arr1 = Int_to_array(num1);
            int[] arr2 = Int_to_array(num2);

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] == arr2[i])
                    correctLocationB++;
                // Array.Exists - Returns true if array contains one or more elements that match the 
                // conditions defined by the specified predicate; otherwise, false.
                else if (Array.Exists(arr2, element => element == arr1[i]))
                    notCorrectLocationW++;
            }

            return new Tuple<int, int>(correctLocationB, notCorrectLocationW);
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

        static int[] getAllCombination()
        {
            //The function returns an array of all 4-digit 
            //numbers that include the digits between 1 and 6 without repeats
            List<int> tempList = new List<int>();
            for (int i = 1234; i <= 6543; i++)
                if (existInGame(i))
                    tempList.Add(i);

            return tempList.ToArray();
        }

        private static bool existInGame(int num)
        {
            //The function receives a 4-digit number
            //and returns whether it is in the game, 
            //ie whether it is between the numbers 1 to 6 without repetitions.

            ArrayList listOfDigitExist = new ArrayList() { 1, 2, 3, 4, 5, 6 };

            while (num > 0)
            {
                listOfDigitExist.Remove(num % 10);
                num /= 10;
            }

            return listOfDigitExist.Count == 2;
        }

        public bool IsMatchCombination(int currentComb)
        {
            // Gets a number and returns true whether it is possible to be the next guess. 
            // Check by comparison with all previous guesses

            for (int i = 0; allGuess[i] != 0 && i < allGuess.Length; i++)
            {
                if (!compareTwoGuesses(allGuess[i], currentComb).Equals(responseToGuess[i]))
                    return false;
            }

            return true;
        }
    }
}
