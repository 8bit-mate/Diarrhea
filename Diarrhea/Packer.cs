namespace Diarrhea
{
    using System.Buffers.Binary;
    using System.Text;
    using System.Text.RegularExpressions;
    using static Globals;

    /// <summary>
    /// Provides methods to pack files into a *.dat container.
    /// </summary>
    /// <param name="numFilesReserved">Number of files reserved in the container.</param>
    /// <param name="regExFilter">RegEx filter to remove prefixes/suffixes from the input file names.</param>
    public class Packer(int numFilesReserved, string regExFilter)
    {
        private readonly int numFilesReserved = numFilesReserved;
        private readonly string regExFilter = regExFilter;
        private readonly int dataOffset = CalcDataOffset(numFilesReserved);

        /// <summary>
        /// Packs a directory with files into a .dat container.
        /// </summary>
        /// <param name="inputDir">Path to the input directory.</param>
        /// <param name="outputPath">Path to the output *.dat container.</param>
        /// <param name="mask">Search pattern, allows to filter files from the input dir.</param>
        /// <exception cref="ArgumentException">Specified input dir not found.</exception>
        public void PackDir(string inputDir, string outputPath, string mask)
        {
            if (Directory.Exists(inputDir))
            {
                DirectoryInfo d = new (inputDir);
                this.PackFiles(d.GetFiles(mask), outputPath);
            }
            else
            {
                throw new ArgumentException($"Directory '{inputDir}' not found.");
            }
        }

        /// <summary>
        /// Pack a list of files into a .dat container.
        /// </summary>
        /// <param name="files">Files to pack.</param>
        /// <param name="outputPath">Path to the output *.dat container.</param>
        /// <exception cref="ArgumentException">The number of input files exceeds the number of files reserved.</exception>
        public void PackFiles(FileInfo[] files, string outputPath)
        {
            var numFiles = files.Length;

            if (this.numFilesReserved < numFiles)
            {
                throw new ArgumentException("The number of input files exceeds the number of files reserved.");
            }

            var header = this.CreateHeader(this.CreateFilesTable(files), numFiles);

            var writeStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            writeStream.Write(header, 0, header.Length);

            foreach (var file in files)
            {
                var buffer = File.ReadAllBytes(file.FullName);
                writeStream.Write(buffer, 0, buffer.Length);
            }

            writeStream.Close();
        }

        private static void PasteBytesToHeader(byte[] srcArr, byte[] dstArr, int initOffset, int idx)
        {
            int srcOffset = 0;
            int byteCount = srcArr.Length;
            int dstOffset = initOffset + (idx * byteCount);

            Buffer.BlockCopy(srcArr, srcOffset, dstArr, dstOffset, byteCount);
        }

        private static bool CheckIfValidFileSize(long value)
        {
            if (value >= 0 && value <= int.MaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string FitStringToLength(string str, int maxLength)
        {
            return str[0..Math.Min(str.Length, maxLength)].PadRight(maxLength, '\0');
        }

        // Calculate initial data offset.
        private static int CalcDataOffset(int numFilesReserved)
        {
            // 2 since we have two int32 arrays of the same length (offsets and sizes).
            return IntSize + (StrSize * numFilesReserved) + (2 * IntSize * numFilesReserved);
        }

        private byte[] CreateHeader(FileEntry[] filesTable, int numFiles)
        {
            var header = new byte[this.dataOffset];
            BinaryPrimitives.TryWriteInt32LittleEndian(header, numFiles);

            for (int i = 0; i < filesTable.Length; i++)
            {
                byte[] fileNameBytes = Encoding.ASCII.GetBytes(filesTable[i].Name);
                byte[] offsetBytes = new byte[IntSize];
                byte[] sizeBytes = new byte[IntSize];

                BinaryPrimitives.TryWriteInt32LittleEndian(offsetBytes, filesTable[i].Offset);
                BinaryPrimitives.TryWriteInt32LittleEndian(sizeBytes, filesTable[i].Size);

                PasteBytesToHeader(fileNameBytes, header, IntSize, i);
                PasteBytesToHeader(offsetBytes, header, IntSize + (StrSize * this.numFilesReserved), i);
                PasteBytesToHeader(sizeBytes, header, IntSize + (StrSize * this.numFilesReserved) + (IntSize * this.numFilesReserved), i);
            }

            return header;
        }

        private FileEntry[] CreateFilesTable(FileInfo[] files)
        {
            var arr = new FileEntry[files.Length];
            var offset = this.dataOffset;

            for (int i = 0; i < files.Length; i++)
            {
                var name = this.FilterFileName(files[i].Name);

                if (!CheckIfValidFileSize(files[i].Length))
                {
                    throw new InvalidFileSizeException(name);
                }

                var size = (int)files[i].Length;
                arr[i] = new FileEntry(name, offset, size);

                offset += size;
            }

            return arr;
        }

        private string FilterFileName(string name)
        {
            return FitStringToLength(
                Regex.Match(name, this.regExFilter).ToString(),
                StrSize);
        }
    }
}