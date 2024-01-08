namespace Wordle_Assignment
{
    // Cian Dicker-Hughes
    // G00415413@atu.ie
    public partial class MainPage : ContentPage
    {
        // Variables
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // move to the game Wordle paga
        private async void MoveToWordle(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Wordle());
        }

        // move to about wordle page
        private async void MoveToAbout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutToWordle());
        }

        // move to the scoreboard page
        private async void Socreboard_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Scoreboard());
        }
    }
}