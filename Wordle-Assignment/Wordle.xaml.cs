namespace Wordle_Assignment;

// Cian Dicker-Hughes
// G00415413@atu.ie

public partial class Wordle : ContentPage
{
	WordsRepository wordsmodel;
	public Wordle()
	{
		InitializeComponent();
		wordsmodel = new ();

		//BindingContext = wordsmodel;

    }

	private async void getWords_Clicked(object sender, EventArgs e)
    {
		await wordsmodel.MakeCollection();
    }
}