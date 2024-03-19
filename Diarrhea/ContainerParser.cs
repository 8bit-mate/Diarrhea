namespace Diarrhea
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Handles binary container parsing.
    /// </summary>
    public class ContainerParser
    {
        private readonly string fileName;
        private readonly int numFilesReserved;
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerParser"/> class.
        /// </summary>
        /// <param name="fileName">Path to the *.dat container</param>
        /// <param name="numFilesReserved">Number of files reserved in the container</param>
        public ContainerParser(string fileName, int numFilesReserved)
        {
            this.fileName = fileName;
            this.numFilesReserved = numFilesReserved;
            this.fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        }

        public void Call()
        {
            int numFilesPacked = this.ReadInt32();
            string[] fileNames = this.ReadStrings(numFilesPacked);
            this.RelJump((this.numFilesReserved - numFilesPacked) * 32);
            int[] offsets = this.ReadIntArray(numFilesPacked);
            this.RelJump((this.numFilesReserved - numFilesPacked) * 4);
            int[] sizes = this.ReadIntArray(numFilesPacked);

            this.fileStream.Close();
        }

        private void RelJump(int nBytes)
        {
            this.fileStream.Seek(nBytes, SeekOrigin.Current);
        }

        private int ReadInt32()
        {
            return BitConverter.ToInt32(this.ReadBytes(4));
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
                filenames[i] = this.ReadString(32);
            }

            return filenames;
        }

        private string ReadString(int strLength)
        {
            byte[] bytes = this.ReadBytes(strLength);
            string str = Encoding.ASCII.GetString(bytes);
            return Regex.Replace(str, @"\s+", string.Empty);
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
    }
 }