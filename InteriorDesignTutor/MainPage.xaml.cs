using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
// using Plugin.Maui.Audio;

namespace InteriorDecorTutor
{
    public partial class MainPage : ContentPage
    {
        private FileResult? _selectedImageFile;
        // private readonly IAudioManager _audioManager;

        public MainPage()//IAudioManager audioManager)
        {
            InitializeComponent();
            // _audioManager = audioManager;
        }

        private async void OnLoadImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select an image"
                });

                if (result != null)
                {
                    _selectedImageFile = result;
                    SelectedImage.Source = ImageSource.FromFile(result.FullPath);
                    GenerateCaptionButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
            }
        }

        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                if (!await CheckAndRequestCameraPermissionsAsync())
                {
                    await DisplayAlert("Error", "Camera permission is required.", "OK");
                    return;
                }

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    _selectedImageFile = photo;
                    SelectedImage.Source = ImageSource.FromFile(photo.FullPath);
                    GenerateCaptionButton.IsEnabled = true;
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "Camera not supported on this device.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Camera permission denied.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unexpected error: {ex.Message}", "OK");
            }
        }

        private async Task<bool> CheckAndRequestCameraPermissionsAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            return status == PermissionStatus.Granted;
        }

        private async void OnGenerateCaptionClicked(object sender, EventArgs e)
        {
            if (_selectedImageFile == null)
            {
                await DisplayAlert("Error", "No image selected.", "OK");
                return;
            }

            try
            {
                // Show loading overlay and start animations
                LoadingOverlay.IsVisible = true;
                LampImage.IsVisible = true;
                PillowImage.IsVisible = true;

                // Animate furniture pieces with bouncy effect
                var lampAnimation = new Animation
                {
                    { 0, 0.5, new Animation(v => LampImage.TranslationX = v, 0, 200, Easing.BounceOut) },
                    { 0, 0.5, new Animation(v => LampImage.TranslationY = v, 0, -100, Easing.BounceOut) },
                    { 0, 1, new Animation(v => LampImage.Rotation = v, 0, 360) },
                    { 0.5, 1, new Animation(v => LampImage.TranslationX = v, 200, 0, Easing.BounceOut) },
                    { 0.5, 1, new Animation(v => LampImage.TranslationY = v, -100, 0, Easing.BounceOut) }
                };

                var pillowAnimation = new Animation
                {
                    { 0, 0.5, new Animation(v => PillowImage.TranslationX = v, 0, -150, Easing.BounceOut) },
                    { 0, 0.5, new Animation(v => PillowImage.TranslationY = v, 0, 120, Easing.BounceOut) },
                    { 0, 1, new Animation(v => PillowImage.Rotation = v, 0, -360) },
                    { 0.5, 1, new Animation(v => PillowImage.TranslationX = v, -150, 0, Easing.BounceOut) },
                    { 0.5, 1, new Animation(v => PillowImage.TranslationY = v, 120, 0, Easing.BounceOut) }
                };

                lampAnimation.Commit(LampImage, "LampAnimation", length: 2500, repeat: () => LoadingOverlay.IsVisible);
                pillowAnimation.Commit(PillowImage, "PillowAnimation", length: 2500, repeat: () => LoadingOverlay.IsVisible);

                // Retrieve the URL from Preferences
                string imageGenerationUrl = Preferences.Get("ImageGenerationUrl", string.Empty);

                if (string.IsNullOrEmpty(imageGenerationUrl))
                {
                    LoadingOverlay.IsVisible = false;
                    LampImage.IsVisible = false;
                    PillowImage.IsVisible = false;
                    await DisplayAlert("Error", "Image generation URL not set. Please configure it in Settings.", "OK");
                    return;
                }

                CaptionLabel.Text = "Generating caption...";

                // Simulate API throttling with a 3-second delay
                await Task.Delay(3000);

                // Placeholder for API call
                /*
                using (var httpClient = new HttpClient())
                {
                    var formData = new MultipartFormDataContent();
                    var imageStream = await _selectedImageFile.OpenReadAsync();
                    formData.Add(new StreamContent(imageStream), "image", _selectedImageFile.FileName);
                    if (!string.IsNullOrEmpty(LlmPromptEntry.Text))
                    {
                        formData.Add(new StringContent(LlmPromptEntry.Text), "prompt");
                    }

                    var response = await httpClient.PostAsync(imageGenerationUrl, formData);
                    response.EnsureSuccessStatusCode();
                    var caption = await response.Content.ReadAsStringAsync();
                    CaptionLabel.Text = caption;
                }
                */

                // Mock caption
                CaptionLabel.Text = $"Generated caption for image using URL: {imageGenerationUrl}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to generate caption: {ex.Message}", "OK");
                CaptionLabel.Text = "Failed to generate caption.";
            }
            finally
            {
                /*// Play blip sound
                try
                {
                    var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("blip.wav"));
                    player.Play();
                }
                catch (Exception ex)
                {
                    // Silently handle audio errors to avoid disrupting the UI
                    Console.WriteLine($"Failed to play blip sound: {ex.Message}");
                } */

                // Hide loading overlay and reset animations
                LoadingOverlay.IsVisible = false;
                LampImage.IsVisible = false;
                PillowImage.IsVisible = false;
                LampImage.TranslationX = 0;
                LampImage.TranslationY = 0;
                LampImage.Rotation = 0;
                PillowImage.TranslationX = 0;
                PillowImage.TranslationY = 0;
                PillowImage.Rotation = 0;
            }
        }
    }
}