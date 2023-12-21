using Microsoft.Maui.Controls.Shapes;

namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Wordle : ContentPage
{
    List<Entry> entryFields = new List<Entry>();
    WordsRepository wordsmodel;
    private Color boardColour = Color.FromArgb("#ffffff");
    private int nextRow = 0;
    private int currentRow = 0;

    public Wordle()
    {
        InitializeComponent();
        wordsmodel = new();
        CreatetheGrid();
        nextRow = 0;

    }

    // just give random word
    private async void getWords_Clicked(object sender, EventArgs e)
    {
        await wordsmodel.MakeCollection();
        string blah = "";
        for (int i = 0; i < wordsmodel.Words.Count && i < 20; i++)
        {
            blah += wordsmodel.Words[i] + "\n";
        }
        //await DisplayAlert("hello", blah, "ok");
        string word = wordsmodel.GetRandomWord();
        await DisplayAlert("This is the word", word, "ok");
    }
    // move to next row and see if guess is correct
    private void wordGuess_Clicked(object sender, EventArgs e)
    {
        nextRow++;
        UpdateRowEnablement();
    }

    // make a grid with enter tags
    public void CreatetheGrid()
    {
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
        // making the border and entry
        for (int i = 0; i < 6; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                // make the Entry
                Entry entry = new Entry
                {
                    Text = "",
                    TextColor = Colors.Red,
                    FontSize = 20,
                    TextTransform = TextTransform.Uppercase,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    MaxLength = 1,
                    IsEnabled = whichrowEnabled(i)
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
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(4, 4, 4, 4)
                    },
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
            }
        }
    }

    // enabled which row
    private bool whichrowEnabled(int col)
    {
        if (col == nextRow)
        {
            return true;
        }
        else
        {
            nextRow = 0;
            return false;
        }
    }

    private void UpdateRowEnablement()
    {
        for (int i = 0; i < 6; i++) // Assuming 6 rows based on your previous code
        {
            bool isRowEnabled = i == currentRow;

            foreach (var entry in entryFields.Where(entry => Grid.GetRow(entry) == i))
            {
                // Explicitly enable the current row and disable others
                entry.IsEnabled = isRowEnabled;
            }
        }
    }

}