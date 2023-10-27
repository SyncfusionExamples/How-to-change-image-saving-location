using System.IO;

namespace ImageEditor_GettingStarted
{
    public interface IDependency
    {
        void SelectDirectory(Stream stream);
    }
}
