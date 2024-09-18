using System;
using System.IO;

namespace Module04Assignment01
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCapturePhotoButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Capture photo using MediaPicker
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    // Open the photo as a stream
                    var stream = await photo.OpenReadAsync();

                    // Set the Image control source to display the photo
                    CapturedImage.Source = ImageSource.FromStream(() => stream);

                    // Optionally, save the photo to cache directory
                    var filePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using var fileStream = File.OpenWrite(filePath);
                    await stream.CopyToAsync(fileStream);
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle case when the camera is not supported on the device
                await DisplayAlert("Error", "Camera is not supported on this device.", "OK");
            }
            catch (PermissionException)
            {
                // Handle permission denied error
                await DisplayAlert("Error", "Camera permission denied.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other errors
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
