using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ImageEditor_GettingStarted.iOS.DependencyImplementation))]
namespace ImageEditor_GettingStarted.iOS
{
    public class DependencyImplementation : UIViewController, IDependency
    {
        /// <summary>
        /// Select directory to save the image stream
        /// </summary>
        /// <param name="stream"></param>
        public void SelectDirectory(Stream stream)
        {
            var bytearray = ReadFully(stream);
            var image = ImageFromBytes(bytearray);
            Save(image);
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

        /// <summary>
        /// Convert byte array into UIImage
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private UIKit.UIImage ImageFromBytes(byte[] bytes)
        {
            try
            {
                Foundation.NSData data = Foundation.NSData.FromArray(bytes);
                UIKit.UIImage image = UIImage.LoadFromData(data);
                UIGraphics.EndImageContext();
                return image;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Save UIImage into directory
        /// </summary>
        /// <param name="image"></param>
        void Save(UIKit.UIImage image)
        {
            var documentsDirectory = Environment.GetFolderPath
                                                (Environment.SpecialFolder.Personal);
            var directoryname = Path.Combine(documentsDirectory, "FolderName");
            Directory.CreateDirectory(directoryname);

            string jpgFile = System.IO.Path.Combine(directoryname, "image.jpg");
            Foundation.NSData imgData = image.AsJPEG();
            Foundation.NSError err = null;
            if (imgData.Save(jpgFile, false, out err))
            {
                Console.WriteLine("saved as" + jpgFile);
            }
            else
            {
                Console.WriteLine("not saved as " + jpgFile + "because" + err.LocalizedDescription);
            }
            var alert = UIAlertController.Create("Saved Location", jpgFile, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            ShowViewController(alert, null);
        }
    }
}