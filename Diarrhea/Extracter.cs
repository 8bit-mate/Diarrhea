namespace Diarrhea
{
    /// <summary>
    /// Provides methods to extract data from a container.
    /// </summary>
    public static class Extracter
    {
        /// <summary>
        /// Extracts all files from a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        /// <param name="dir">Output directory.</param>
        /// <param name="nameWrapper">Allows to wrap each file name with a prefix and a suffix.</param>
        public static void ExtractAll(ContainerParser parser, string dir, string[] nameWrapper)
        {
            var dataTable = parser.Parse();
            ExtractData(dataTable, parser.FileName, dir, nameWrapper);
        }

        /// <summary>
        /// Extracts list of files (spec. by the names) from a container.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        /// <param name="fileNames">Files to extract.</param>
        /// <param name="dir">Output directory.</param>
        /// <param name="nameWrapper">Allows to wrap each file name with a prefix and a suffix.</param>
        public static void ExtractList(ContainerParser parser, List<string> fileNames, string dir, string[] nameWrapper)
        {
            var dataTable = parser.Parse();
            IEnumerable<FileEntry> selectedFiles = dataTable.Where(e => fileNames.Contains(e.Name));
            ExtractData(selectedFiles.ToArray(), parser.FileName, dir, nameWrapper);
        }

        private static void ExtractData(FileEntry[] dataTable, string inputPath, string dir, string[] nameWrapper)
        {
            var readStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            foreach (var e in dataTable)
            {
                byte[] buffer = new byte[e.Size];
                readStream.Seek(e.Offset, SeekOrigin.Begin);
                readStream.Read(buffer, 0, e.Size);

                WriteFile($"{dir}\\{nameWrapper[0]}{e.Name}{nameWrapper[1]}", buffer);
            }

            readStream.Close();
        }

        private static void WriteFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
    }
}