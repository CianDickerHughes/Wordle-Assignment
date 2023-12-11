using Microsoft.Maui.Controls.Shapes;

namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Wordle : ContentPage
{
	WordsRepository wordsmodel;
    private Color boardColour = Color.FromArgb("#ffffff");

    public Wordle()
	{
		InitializeComponent();
		wordsmodel = new ();
        CreatetheGrid();

        //BindingContext = wordsmodel;

    }

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

    public void CreatetheGrid()
    {
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                Border border = new Border
                {
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
                        Text = "",
                        TextColor = Colors.Red,
                        FontSize = 10,
                        FontAttributes = FontAttributes.Bold,
                        //VerticalOptions = LayoutOptions.Center,
                        //HorizontalOptions = horizontaloption(i)
                    }
                };
                GridGameTable.Add(border, j, i);
            }
        }
    }
}