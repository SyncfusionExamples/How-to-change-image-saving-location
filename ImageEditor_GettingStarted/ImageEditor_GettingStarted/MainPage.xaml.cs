using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ImageEditor_GettingStarted
{

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            imageeditor.Source = ImageSource.FromResource("ImageEditor_GettingStarted.Image.jpg");
            imageeditor.ImageSaving -= Imageeditor_ImageSaving;
            imageeditor.ImageSaving += Imageeditor_ImageSaving;
        }

        private void Imageeditor_ImageSaving(object sender, ImageSavingEventArgs args)
        {
            args.Cancel = true;
            DependencyService.Get<IDependency>().SelectDirectory(args.Stream);
        }
    }

}
