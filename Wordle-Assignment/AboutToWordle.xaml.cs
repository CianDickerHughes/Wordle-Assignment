namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie
public partial class AboutToWordle : ContentPage
{
	public AboutToWordle()
	{
		InitializeComponent();
	}

    // move to the game Wordle paga
    private async void MoveToWordle(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Wordle());
    }
}