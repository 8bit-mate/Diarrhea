namespace Diarrhea
{
    /// <summary>
    /// Provides methods to extract data from a container.
    /// </summary>
    public class Extracter
    {
        /// <summary>
        /// Extracts all files from a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        /// <param name="dir">Output directory.</param>
        /// <param name="suffix">String to add at the end of the extracted file names.</param>
        public static void ExtractAll(ContainerParser parser, string dir, string suffix)
        {
            var dataTable = parser.Parse();
            ExtractData(dataTable, parser.FileName, dir, suffix);
        }

        /// <summary>
        /// Extracts list of files (spec. by the names) from a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        /// <param name="fileNames">Files to extract.</param>
        /// <param name="dir">Output directory.</param>
        /// <param name="suffix">String to add at the end of the extracted file names.</param>
        public static void ExtractList(ContainerParser parser, List<string> fileNames, string dir, string suffix)
        {
            var dataTable = parser.Parse();
            IEnumerable<FileEntry> selectedFiles = dataTable.Where(e => fileNames.Contains(e.Name));
            ExtractData(selectedFiles.ToArray(), parser.FileName, dir, suffix);
        }

        private static void ExtractData(FileEntry[] dataTable, string inputPath, string dir, string suffix)
        {
            var readStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);

            foreach (var e in dataTable)
            {
                byte[] buffer = new byte[e.Size];
                readStream.Seek(e.Offset, SeekOrigin.Begin);
                readStream.Read(buffer, 0, e.Size);

                WriteFile($"{dir}\\{e.Name}{suffix}", buffer);
            }

            readStream.Close();
        }

        private static void WriteFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
    }
}