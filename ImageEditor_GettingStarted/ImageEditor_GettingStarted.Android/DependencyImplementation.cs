using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Syncfusion.SfImageEditor.XForms;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ImageEditor_GettingStarted.Droid.DependencyImplementation))]
namespace ImageEditor_GettingStarted.Droid
{
    public class DependencyImplementation : IDependency
    {
        public static readonly int REQUEST_CODE_OPEN_DIRECTORY = 1;
        MainActivity activity = new MainActivity();
        Android.Net.Uri FolderPath;
        string OriginalPath;
        string RealPath;
        Stream str = null;

        /// <summary>
        /// Select directory to save the image stream
        /// </summary>
        /// <param name="stream"></param>
        public void SelectDirectory(Stream stream)
        {
            activity = MainActivity.Instance;
            activity.Intent = new Intent();
            activity.Intent.SetAction(Intent.ActionOpenDocumentTree);
            activity.StartActivityForResult(activity.Intent, REQUEST_CODE_OPEN_DIRECTORY);
            activity.ActivityResult += (object sender, ActivityResultEventArgs e) =>
            {
                FolderPath = e.Intent.Data;
                string DummyPath = FolderPath.Path;
                OriginalPath = DummyPath.Split(':')[1];
                RealPath = "/storage/emulated/0/" + OriginalPath;
                Save();
            };
            str = stream;
            Toast.MakeText(activity, "Image has been saved into" + RealPath, ToastLength.Short).Show();
        }

        /// <summary>
        /// Convert stream into bitmap
        /// </summary>
        public void Save()
        {
            str.Position = 0;
            var fileBytes = ReadFully(str);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(fileBytes, 0, fileBytes.Length);
            ExportBitmapAsJPG(bitmap);
        }

        /// <summary>
        /// convert bitmap toJPG
        /// </summary>
        void ExportBitmapAsJPG(Bitmap bitmap)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(RealPath, "image.jpg");
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            stream.Close();
        }

        /// <summary>
        /// Convert Stream into byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}