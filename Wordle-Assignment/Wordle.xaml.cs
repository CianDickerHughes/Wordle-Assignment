using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Wordle : ContentPage
{
    // Variables
    List<Entry> entryFields = new List<Entry>();
    List<string> entryTextList = new List<string>();
    List<string> userWords = new List<string>();
    WordsRepository wordsmodel;
    CheckWord wordCheck;
    private Color boardColour = Color.FromArgb("#696969");
    private string theWord = "";
    private int nextRow = 0, whichrow = 0;

    public Wordle()
    {
        InitializeComponent();
        wordsmodel = new();
        CreatetheGrid();
        getRandonWord();


    }

    // get the user word
    public List<string> GetUserWord() 
    {
        int maxWhichrow = whichrow + 5;
        for (int i = whichrow; i < maxWhichrow && i < entryTextList.Count; i++) 
        {
            userWords.Add(entryTextList[i]);
        }
        return userWords;
    }
    // get the word
    public string GetWord()
    {
        return theWord;
    }

    // just give random word just for debuging
    private async void getWords_Clicked(object sender, EventArgs e)
    {
        string word = wordCheck.GetIsItCorrect();
        await DisplayAlert("This is the word", word, "ok");
    }

    // getting the word to start game
    private async void getRandonWord()
    {
        await wordsmodel.MakeCollection();
        theWord = wordsmodel.GetRandomWord();
        await DisplayAlert("This is the word", theWord, "ok");
    }

    // move to next row and see if guess is correct
    private void WordGuess_Clicked(object sender, EventArgs e)
    {
        int initialCount = entryTextList.Count;

        // Loop through each Entry in the grid and collect the words
        foreach (Entry entry in entryFields.Skip(initialCount))
        {
            // get the letters for the entry in the grid
            if (!string.IsNullOrWhiteSpace(entry.Text))
            { 
                    entryTextList.Add(entry.Text);
            }
        }

        wordCheck = new();

        // make grid with text and move to new row
        GridGameTable.Children.Clear(); 
        nextRow++;
        CreatetheGrid();
    }

    // make a grid with enter tags
    public void CreatetheGrid()
    {
        // is it on moble or destop
        double devicewidth = Preferences.Default.Get("devicewidth", 480.0);
        if (devicewidth < 480)
        {
            int newwidth = ((int)devicewidth / 5) * 10;
            GridGameTable.WidthRequest = newwidth;
            GridGameTable.HeightRequest = ((int)devicewidth / 6) * 10;
        }
        int margin = 0;
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            margin = -2;

        int entryTextIndex = 0;
        entryFields.Clear();

        // making the border and entry
        for (int i = 0; i < 6; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                string entryText = entryTextIndex < entryTextList.Count ? entryTextList[entryTextIndex] : ""; // Get the text from entryTextList or set it to blank if out of range
                // make the Entry
                Entry entry = new Entry
                {
                    Text = entryText,
                    TextColor = Colors.White,
                    FontSize = 20,
                    TextTransform = TextTransform.Uppercase,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    MaxLength = 1,
                    IsEnabled = WhichrowEnabled(i)
                };
                entry.TextChanged += (sender, args) =>
                {
                    // Check if a character is entered
                    if (args.NewTextValue.Length > 0)
                    {
                        var currentEntry = (Entry)sender;
                        int currentIndex = entryFields.IndexOf(currentEntry);
                        // Move focus to the next entry
                        if (currentIndex < entryFields.Count - 1)
                        {
                            var nextEntry = entryFields[currentIndex + 1];
                            nextEntry.Focus();
                        }
                    }
                };

                // making the border
                Border border = new Border
                {
                    Margin = margin,
                    StrokeThickness = 2,
                    Background = boardColour,
                    Padding = new Thickness(3, 3),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Stroke = new LinearGradientBrush
                    {
                        EndPoint = new Point(0, 1),
                        GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Black, Offset = 0.1f },
                        new GradientStop { Color = Colors.Black, Offset = 1.0f }
                    },
                    },
                    Content = entry
                };
                entryFields.Add(entry);
                GridGameTable.Add(border, j, i);
                entryTextIndex++;
            }
        }
    }

    // enabled which row
    private bool WhichrowEnabled(int col)
    {
        if (col == nextRow)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}