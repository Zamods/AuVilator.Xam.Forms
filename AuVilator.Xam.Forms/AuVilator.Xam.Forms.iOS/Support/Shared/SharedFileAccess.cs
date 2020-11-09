using AuVilator.Xam.Forms.iOS.Support.Local;
using AuVilator.Xam.Forms.iOS.Support.Shared;
using AuVilator.Xam.Forms.Support.Interface;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(SharedFileAccess))]

namespace AuVilator.Xam.Forms.iOS.Support.Shared
{
    public class SharedFileAccess : IFileAccess
    {
        public string AcquireFilePath(string fileName, Environment.SpecialFolder selectedFolder)
        {
            return FileAccess.GenerateFilePath(fileName: fileName, selectedFolder: selectedFolder);
        }

        public string[] DirectoryFilesPath(Environment.SpecialFolder selectedFolder)
        {
            return FileAccess.DirectoryFilesPath(selectedFolder);
        }
    }
}