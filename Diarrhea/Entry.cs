namespace Diarrhea
{
    public readonly struct Entry
    {
        public Entry(string filename, uint offset, uint size)
        {
            this.Filename = filename;
            this.Offset = offset;
            this.Size = size;
        }

        public string Filename { get; init; }

        public uint Offset { get; init; }

        public uint Size { get; init; }
    }
}