﻿namespace Wordle_Assignment
{
    // Cian Dicker-Hughes
    // G00415413@atu.ie
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void MoveToWordle(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Wordle());
        }

        private async void MoveToAbout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutToWordle());
        }
    }
}