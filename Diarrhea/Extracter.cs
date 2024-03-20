namespace Diarrhea
{
    /// <summary>
    /// Provides methods to extract data from a container.
    /// </summary>
    public class Extracter
    {
        /// <summary>
        /// Extracts all files from a container to single files.
        /// </summary>
        /// <param name="parser">Parser instance.</param>
        /// <param name="dir">Output directory.</param>
        public static void ExtractAll(ContainerParser parser, string dir, string suffix)
        {
            var dataTable = parser.Parse();

            var readStream = new FileStream(parser.FileName, FileMode.Open, FileAccess.Read);

            foreach (var (fileName, offset, size) in dataTable)
            {
                byte[] buffer = new byte[size];
                readStream.Seek(offset, SeekOrigin.Begin);
                readStream.Read(buffer, 0, size);

                WriteFile($"{dir}\\{fileName}{suffix}", buffer);
            }

            readStream.Close();
        }

        private static void WriteFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
    }
}