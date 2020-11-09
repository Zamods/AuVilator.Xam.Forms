using Android.OS;
using Java.Net;
using System.IO;

namespace AuVilator.Xam.Forms.Droid.Support.Local
{
    public class FileAccess
    {
        /// <summary>
        /// Generates the platform specific file path with given system folder.
        /// </summary>
        /// <param name="fileName">Name of the file including the format.</param>
        /// <param name="selectedFolder">Platform specific system folder.</param>
        /// <returns>Complete file path in [String] format.</returns>
        public static string GenerateFilePath(string fileName, System.Environment.SpecialFolder selectedFolder)
        {
            string path = Environment.ExternalStorageDirectory.AbsolutePath;
            string dirPath = System.IO.Path.Combine(path, "AuVilator");
            if (!Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(path, "AuVilator"));
            }
            return System.IO.Path.Combine(dirPath, fileName);
        }
        /// <summary>
        /// Generates the platform specific file path with given system folder.
        /// </summary>
        /// <param name="fileName">Name of the file including the format.</param>
        /// <param name="selectedFolder">Platform specific system folder.</param>
        /// <returns>Complete file path in [URI] format.</returns>
        public static URI GenerateFileURI(string fileName, System.Environment.SpecialFolder selectedFolder)
        {
            string path = System.Environment.GetFolderPath(selectedFolder);
            return new URI(System.IO.Path.Combine(path, fileName));
        }

        /// <summary>
        /// Generates the paths of all consisting files in given directory
        /// </summary>
        /// <param name="selectedFolder"></param>
        /// <returns>List of file paths in [string] array format.</returns>
        public static string[] DirectoryFilesPath(System.Environment.SpecialFolder selectedFolder)
        {
            string folderPath = System.Environment.GetFolderPath(selectedFolder);
            return System.IO.Directory.GetFiles(folderPath);
        }
    }
}