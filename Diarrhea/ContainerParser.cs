namespace Diarrhea
{
    using System.Text;
    using System.Text.RegularExpressions;
    using static Globals;

    /// <summary>
    /// Provides methods to parse a binary container.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ContainerParser"/> class.
    /// </remarks>
    /// <param name="fileName">Path to the *.dat container</param>
    /// <param name="numFilesReserved">Number of files reserved in the container</param>
    public partial class ContainerParser(string fileName, int numFilesReserved)
    {
        private readonly string fileName = fileName;
        private readonly int numFilesReserved = numFilesReserved;
        private readonly FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

        /// <summary>
        /// Gets filename of a parsing file.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// Parse binary container to get file names, offsets and sizes.
        /// </summary>
        /// <returns>Array of objects with properties required to extract data.</returns>
        public FileEntry[] Parse()
        {
            int numFilesPacked = this.ReadInt32();
            string[] fileNames = this.ReadStrings(numFilesPacked);
            this.RelJump((this.numFilesReserved - numFilesPacked) * StrSize);
            int[] offsets = this.ReadIntArray(numFilesPacked);
            this.RelJump((this.numFilesReserved - numFilesPacked) * IntSize);
            int[] sizes = this.ReadIntArray(numFilesPacked);

            this.fileStream.Close();

            return fileNames.Zip(offsets, sizes).Select(tuple => new FileEntry(
                    tuple.First,
                    tuple.Second,
                    tuple.Third))
                .ToArray();
        }

        private void RelJump(int nBytes)
        {
            this.fileStream.Seek(nBytes, SeekOrigin.Current);
        }

        private int ReadInt32()
        {
            return BitConverter.ToInt32(this.ReadBytes(IntSize));
        }

        private byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];

            this.fileStream.Read(buffer, 0, length);

            return buffer;
        }

        private string[] ReadStrings(int numFilesPacked)
        {
            string[] filenames = new string[numFilesPacked];

            for (int i = 0; i < numFilesPacked; i++)
            {
                filenames[i] = this.ReadString(StrSize);
            }

            return filenames;
        }

        private string ReadString(int strLength)
        {
            byte[] bytes = this.ReadBytes(strLength);
            string str = Encoding.ASCII.GetString(bytes);
            return RemoveNullsRegex().Replace(str, string.Empty);
        }

        private int[] ReadIntArray(int numFilesPacked)
        {
            int[] arr = new int[numFilesPacked];

            for (int i = 0; i < numFilesPacked; i++)
            {
                arr[i] = this.ReadInt32();
            }

            return arr;
        }

        [GeneratedRegex("\0")]
        private static partial Regex RemoveNullsRegex();
    }
 }