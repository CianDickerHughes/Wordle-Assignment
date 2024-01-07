using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle_Assignment
{
    // Cian Dicker-Hughes
    // G00415413@atu.ie

    public class CheckWord
    {
        // Variables
        Wordle wordsModel;
        List<string> userWords = new List<string>();
        private string theWord = "";
        private string isCorrect = "";

        public CheckWord()
        {
            wordsModel = new();
            GetTheUserWord();
            GetTheWord();

        }

        // get the user word from the wordle.xaml.cs
        public void GetTheUserWord()
        {
            for (int i = 0; i < 5; i++)
            {
                userWords = wordsModel.GetUserWord();
            }
        }

        // get the word from the wordle.xaml.cs
        public void GetTheWord()
        {
            theWord = wordsModel.GetWord();
        }

        // checks if the user got the right word
        public void ChecktheWord()
        {
            GetTheUserWord();
            GetTheWord();
            for (int i = 0; i < 5; i++) 
            {
                if (userWords[i].Equals(theWord[i]))
                {
                    isCorrect = "1";
                }
                else if (theWord.Contains(userWords[i]))
                {
                    isCorrect = "2";
                }
                else 
                {
                    isCorrect = "3";
                }
            
            }
        }

        public string GetIsItCorrect() 
        {
            ChecktheWord();
            return isCorrect;
        }

    }
}
