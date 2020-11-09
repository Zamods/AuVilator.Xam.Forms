namespace AuVilator.Xam.Forms.Support.Interface
{
    public interface IFileAccess
    {
        /// <summary>
        /// Acquires the platform specific file path with given system folder.
        /// </summary>
        /// <param name="fileName">Name of the file including the format.</param>
        /// <param name="selectedFolder">Platform specific system folder.</param>
        /// <returns>Complete file path in [String] format.</returns>
        string AcquireFilePath(string fileName, System.Environment.SpecialFolder selectedFolder);

        /// <summary>
        /// Generates the paths of all consisting files in given directory
        /// </summary>
        /// <param name="selectedFolder"></param>
        /// <returns>List of file paths in [string] array format.</returns>
        string[] DirectoryFilesPath(System.Environment.SpecialFolder selectedFolder);
    }
}