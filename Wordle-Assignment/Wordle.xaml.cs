using Microsoft.Maui.Controls.Shapes;
using System.Collections.Generic;
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
    List<int> correctnessStatus = new List<int>() { 3, 3, 3, 3, 3 };
    WordsRepository wordsmodel;
    CheckWord wordCheck;
    private Color boardColour = Color.FromArgb("#696969");
    private string theWord = "tom";
    private int nextRow = -1, whichrow = 0;

    public Wordle()
    {
        InitializeComponent();
        CreatetheGrid(correctnessStatus);
        wordGuess.IsEnabled = false;
    }

    // getting the word to start game
    private async void getRandonWord()
    {
        wordsmodel = new();
        await wordsmodel.MakeCollection();
        this.theWord = wordsmodel.GetRandomWord();
    }

    // to start the game
    private void StartBtn_Clicked(object sender, EventArgs e)
    {
        getRandonWord();
        StartBtn.IsEnabled = false;
        wordGuess.IsEnabled = true;
        nextRow++;
        CreatetheGrid(correctnessStatus);
        correctnessStatus.Clear();
        var wordCheck = new CheckWord(theWord);
        dubug2.Text = theWord.ToString();
    }

    private async void WordleWin() 
    {
        int win = 0;
        int maxWhichrow = whichrow;
        for (int i = whichrow - 5; i < maxWhichrow && i < correctnessStatus.Count; i++)
        {
           win += correctnessStatus[i];
        }
        if (win == 5)
        {
            await DisplayAlert("win", win.ToString(), "ok");
        }
        else 
        {
            await DisplayAlert("no", win.ToString(), "ok");
        }
    }

    // check if the row is filled
    private bool AreEntryBoxesFilledInRow(int row)
    {
        int startIndex = row * 5;
        int endIndex = startIndex + 5;

        for (int i = startIndex; i < endIndex; i++)
        {
            if (i >= entryFields.Count || string.IsNullOrWhiteSpace(entryFields[i].Text))
            {
                return false;
            }
        }
        return true; 
    }

    // move to next row and see if guess is correct
    private void WordGuess_Clicked(object sender, EventArgs e)
    {
        if (!AreEntryBoxesFilledInRow(nextRow))
        {
            return;
        }

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

        // send to word check 
        GetUserWord();
        var wordCheck = new CheckWord(theWord, userWords);
        List<int> isCorrect = wordCheck.GetIsItCorrect();
        for (int i = 0; i < 5; i++) {
            correctnessStatus.Add(isCorrect[i]);
        }

        WordleWin();

        // make grid with text and move to new row
        GridGameTable.Children.Clear(); 
        nextRow++;
        CreatetheGrid(correctnessStatus);
    }

    // make a grid with enter tags
    public void CreatetheGrid(List<int> correctnessStatus)
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
        int index = 0;

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

                // change colour if its row
                if (index < correctnessStatus.Count)
                {
                    if (nextRow >= 0 && nextRow < 6)
                    {
                        // Get the colour for the background based on correctnessStatus
                        switch (correctnessStatus[index])
                        {
                            case 1:
                                boardColour = Color.FromArgb("#00FF00"); // Green color
                                break;
                            case 2:
                                boardColour = Color.FromArgb("#FFD700"); // Yellow color
                                break;
                            default:
                                boardColour = Color.FromArgb("#696969"); // Grey color
                                break;
                        }
                        index++; // Increment the index for the next iteration
                    }
                }
                else 
                {
                    boardColour = Color.FromArgb("#696969");
                }


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

    // get the user word
    public List<string> GetUserWord()
    {
        int maxWhichrow = whichrow + 5;
        userWords.Clear();
        for (int i = whichrow; i < maxWhichrow && i < entryTextList.Count; i++)
        {
            userWords.Add(entryTextList[i]);
        }
        whichrow = maxWhichrow;
        return userWords;
    }
}