namespace Diarrhea
{
    /// <summary>
    /// Attributes of a file entry to pack/extract.
    /// </summary>
    /// <param name="name">File name.</param>
    /// <param name="offset">File offset in the container.</param>
    /// <param name="size">File size.</param>
    public class FileEntry(string name, int offset, int size)
    {
        private readonly string name = name;
        private readonly int offset = offset;
        private readonly int size = size;

        public string Name
        {
            get { return this.name; }
        }

        public int Offset
        {
            get { return this.offset; }
        }

        public int Size
        {
            get { return this.size; }
        }
    }
}