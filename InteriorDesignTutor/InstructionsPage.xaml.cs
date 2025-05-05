using Microsoft.Maui.Controls;

namespace InteriorDecorTutor
{
    public partial class InstructionsPage : ContentPage
    {
        public InstructionsPage()
        {
            InitializeComponent();
        }

        private async void OnGetStartedClicked(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}