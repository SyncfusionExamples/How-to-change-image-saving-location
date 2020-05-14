using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ImageEditor_GettingStarted.UWP.DependencyImplementation))]
namespace ImageEditor_GettingStarted.UWP
{
    public class DependencyImplementation : IDependency
    {
        SfImageEditor editor = new SfImageEditor();
        Stream stream;

        /// <summary>
        /// Select directory to save the image stream
        /// </summary>
        /// <param name="stream"></param>
        public async void SelectDirectory(Stream stream)
        {
            this.stream = stream;
            IRandomAccessStream randomAccessStream = this.stream.AsRandomAccessStream();
            var wbm = new WriteableBitmap(600, 800);
            await wbm.SetSourceAsync(randomAccessStream);
            string fileName = "image.jpg";
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            string SelectedFolderPath = folder.Path;
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (var storageStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, storageStream);
                    var pixelStream = wbm.PixelBuffer.AsStream();
                    var pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)wbm.PixelWidth, (uint)wbm.PixelHeight, 200, 200, pixels);
                    await encoder.FlushAsync();
                }
            }
        }
    }
}