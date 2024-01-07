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
        List<string> userWords = new List<string>();
        List<int> isCorrect = new List<int>();
        public string theWord { get; set; } = "hi";
        

        public CheckWord()
        {
            
        }

        public CheckWord(string theWord)
        {
            this.theWord = theWord; 
        }

        public CheckWord(string theWord, List<string> userWords)
        {
            this.theWord = theWord;
            this.userWords = userWords;
            int i = 0;
        }

        // checks if the user got the right word
        public void ChecktheWord()
        {
            //SetTheUserWord();
            //SetTheWord();
            isCorrect.Clear(); // Clear the previous correctness status

            for (int i = 0; i < userWords.Count; i++)
            {
               
                string userWord = userWords[i];
                string tempWord = theWord;

                for (int j = 0; j < userWord.Length; j++)
                {
                    if (char.ToUpper(theWord[i]) == char.ToUpper(userWords[i][0]))
                    {
                        isCorrect.Add(1); // Indicates a letter in the correct position
                    }
                    else if (theWord.IndexOf(char.ToUpper(userWords[i][0]), StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        isCorrect.Add(2); // Indicates a correct letter in the wrong position
                    }
                    else
                    {
                        isCorrect.Add(3); // Indicates no match
                    }
                }
            }
        }

        public List<int> GetIsItCorrect() 
        {
            ChecktheWord();
            return isCorrect;
        }
    }
}

// string userWords = userWords[i].toCharArray();