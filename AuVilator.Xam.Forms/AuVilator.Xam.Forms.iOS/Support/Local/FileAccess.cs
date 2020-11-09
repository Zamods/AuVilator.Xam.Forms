using System;

namespace AuVilator.Xam.Forms.iOS.Support.Local
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
            string docFolder = Environment.GetFolderPath(selectedFolder);
            string libFolder = System.IO.Path.Combine(docFolder, "..", "Documents");
            IsDirectoryExists(libFolder);
            return System.IO.Path.Combine(libFolder, fileName);
        }

        /// <summary>
        /// Generates the platform specific file path with given system folder.
        /// </summary>
        /// <param name="fileName">Name of the file including the format.</param>
        /// <param name="selectedFolder">Platform specific system folder.</param>
        /// <returns>Complete file path in [URI] format.</returns>
        public static Uri GenerateFileURI(string fileName, System.Environment.SpecialFolder selectedFolder)
        {
            string docFolder = Environment.GetFolderPath(selectedFolder);
            string libFolder = System.IO.Path.Combine(docFolder, "..", "Documents");
            IsDirectoryExists(libFolder);
            return new Uri(System.IO.Path.Combine(libFolder, fileName));
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

        private static void IsDirectoryExists(string libFolder)
        {
            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }
        }
    }
}