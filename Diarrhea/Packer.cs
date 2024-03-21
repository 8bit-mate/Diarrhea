namespace Diarrhea
{
    using System.Buffers.Binary;
    using System.Text;
    using static Globals;

    /// <summary>
    /// Provides methods to pack files into a *.dat container.
    /// </summary>
    public static class Packer
    {
        /// <summary>
        /// Packs a directory with files into a .dat container.
        /// </summary>
        /// <param name="inputDir">Path to the input directory.</param>
        /// <param name="outputPath">Path to the output *.dat container.</param>
        /// <param name="mask">Search pattern, allows to filter files from the input dir.</param>
        /// <param name="numFilesReserved">How many files the output container can hold.</param>
        /// <exception cref="ArgumentException">Specified input dir not found.</exception>
        public static void PackDir(string inputDir, string outputPath, string mask, int numFilesReserved)
        {
            if (Directory.Exists(inputDir))
            {
                DirectoryInfo d = new (inputDir);
                PackFiles(d.GetFiles(mask), outputPath, numFilesReserved);
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
        /// <param name="numFilesReserved">How many files the output container can hold.</param>
        /// <exception cref="ArgumentException">The number of input files exceeds the number of files reserved.</exception>
        public static void PackFiles(FileInfo[] files, string outputPath, int numFilesReserved)
        {
            var numFiles = files.Length;

            if (numFilesReserved < numFiles)
            {
                throw new ArgumentException("The number of input files exceeds the number of files reserved.");
            }

            var header = CreateHeader(CreateFilesTable(files, numFilesReserved), numFilesReserved, numFiles);

            var writeStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            writeStream.Write(header, 0, header.Length);

            foreach (var file in files)
            {
                var buffer = File.ReadAllBytes(file.FullName);
                writeStream.Write(buffer, 0, buffer.Length);
            }

            writeStream.Close();
        }

        private static byte[] CreateHeader(FileEntry[] filesTable, int numFilesReserved, int numFiles)
        {
            var arr = new byte[CalcDataOffset(numFilesReserved)];
            BinaryPrimitives.TryWriteInt32LittleEndian(arr, numFiles);

            for (int i = 0; i < filesTable.Length; i++)
            {
                byte[] fileNamesBytes = Encoding.ASCII.GetBytes(filesTable[i].Name);
                byte[] offsetBytes = new byte[IntSize];
                BinaryPrimitives.TryWriteInt32LittleEndian(offsetBytes, filesTable[i].Offset);
                byte[] sizeBytes = new byte[IntSize];
                BinaryPrimitives.TryWriteInt32LittleEndian(sizeBytes, filesTable[i].Size);

                CopyBytes(fileNamesBytes, arr, IntSize, i, StrSize);
                CopyBytes(offsetBytes, arr, IntSize + (StrSize * numFilesReserved), i, IntSize);
                CopyBytes(sizeBytes, arr, IntSize + (StrSize * numFilesReserved) + (IntSize * numFilesReserved), i, IntSize);
            }

            return arr;
        }

        private static void CopyBytes(byte[] src, byte[] dst, int offset, int idx, int length)
        {
            for (int j = 0; j < src.Length; j++)
            {
                dst[offset + (idx * length) + j] = src[j];
            }
        }

        private static FileEntry[] CreateFilesTable(FileInfo[] files, int numFilesReserved)
        {
            var arr = new FileEntry[files.Length];

            int offset = CalcDataOffset(numFilesReserved);

            for (int i = 0; i < files.Length; i++)
            {
                var name = TruncateLongString(files[i].Name, StrSize);
                var size = (int)files[i].Length; // todo: throw ex. if the value exceeds int32.

                arr[i] = new FileEntry(name, offset, size);

                offset += (int)files[i].Length;  // todo: throw ex. if the value exceeds int32.
            }

            return arr;
        }

        private static string? TruncateLongString(this string str, int maxLength)
        {
            return str?[0..Math.Min(str.Length, maxLength)];
        }

        private static int CalcDataOffset(int numFilesReserved)
        {
            // 2 since we have two int32 arrays of the same length (offsets and sizes).
            return IntSize + (StrSize * numFilesReserved) + (2 * IntSize * numFilesReserved);
        }
    }
}