using System;
using Microsoft.Maui.Controls;

namespace InteriorDecorTutor
{
    public partial class SettingsPage : ContentPage
    {
        public string ImageGenerationUrl
        {
            get => Preferences.Get(nameof(ImageGenerationUrl), string.Empty);
            set => Preferences.Set(nameof(ImageGenerationUrl), value);
        }

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (Uri.TryCreate(UrlEntry.Text, UriKind.Absolute, out _))
            {
                ImageGenerationUrl = UrlEntry.Text;
                await DisplayAlert("Success", "URL saved successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid URL.", "OK");
            }
        }
    }
}