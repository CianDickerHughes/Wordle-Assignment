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
        await DisplayAlert("hello", word, "ok");
    }

    // make a grid with enter tags
    public void CreatetheGrid() { 
        double devicewidth = Preferences.Default.Get("devicewidth", 480.0);
        if(devicewidth< 480) {
            int newwidth = ((int)devicewidth / 5) * 10;
            GridGameTable.WidthRequest = newwidth;
            GridGameTable.HeightRequest = ((int) devicewidth / 6) * 10;
        }
        int margin = 0;
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            margin = -2;
        for (int i = 0; i < 6; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
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
                    Content = new Entry
                    {
                        //X:Name = "entry",
                        Text = "",
                        TextColor = Colors.Red,
                        FontSize = 20,
                        TextTransform = TextTransform.Uppercase,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        MaxLength = 1,
                        IsEnabled = whichrowEnabled(i),
                        

                        //Keyboard.Chat
                    }
                };
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