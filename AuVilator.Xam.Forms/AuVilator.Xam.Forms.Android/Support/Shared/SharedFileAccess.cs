using AuVilator.Xam.Forms.Droid.Support.Local;
using AuVilator.Xam.Forms.Droid.Support.Shared;
using AuVilator.Xam.Forms.Support.Interface;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(SharedFileAccess))]

namespace AuVilator.Xam.Forms.Droid.Support.Shared
{
    public class SharedFileAccess : IFileAccess
    {
        public string AcquireFilePath(string fileName, System.Environment.SpecialFolder selectedFolder)
        {
            return FileAccess.GenerateFilePath(fileName: fileName, selectedFolder: selectedFolder);
        }

        public string[] DirectoryFilesPath(Environment.SpecialFolder selectedFolder)
        {
            return FileAccess.DirectoryFilesPath(selectedFolder: selectedFolder);
        }
    }
}