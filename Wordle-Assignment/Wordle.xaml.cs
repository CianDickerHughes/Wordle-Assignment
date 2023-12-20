using Microsoft.Maui.Controls.Shapes;

namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Wordle : ContentPage
{
	WordsRepository wordsmodel;
    private Color boardColour = Color.FromArgb("#ffffff");
    private int nextRow;

    public Wordle()
	{
		InitializeComponent();
		wordsmodel = new ();
        CreatetheGrid();
        nextRow = 0;

    }

    // just give random word
	private async void getWords_Clicked(object sender, EventArgs e)
    {
		await wordsmodel.MakeCollection();
		string blah = "";
		for(int i=0; i<wordsmodel.Words.Count && i<20;  i++)
		{
			blah += wordsmodel.Words[i] + "\n"; 
		}
		//await DisplayAlert("hello", blah, "ok");
		string word = wordsmodel.GetRandomWord();
        await DisplayAlert("This is the word", word, "ok");
    }

    // make a grid with enter tags
    public void CreatetheGrid()
    {
        List<Entry> entryFields = new List<Entry>();
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
        for (int i = 0; i < 6; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
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
                    if (args.NewTextValue.Length > 0) // Check if a character is entered
                    {
                        var currentEntry = (Entry)sender;
                        int currentIndex = entryFields.IndexOf(currentEntry);
                        if (currentIndex < entryFields.Count - 1)
                        {
                            var nextEntry = entryFields[currentIndex + 1];
                            nextEntry.Focus(); // Move focus to the next entry
                        }
                    }
                };

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
        if (col == nextRow) {
            return true;
        }
        else {
            nextRow = 0;
            return false;
        }
    }

}