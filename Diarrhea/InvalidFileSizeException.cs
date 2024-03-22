namespace Diarrhea
{
    /// <summary>
    /// Invalid file size.
    /// </summary>
    public class InvalidFileSizeException : Exception
    {
        public InvalidFileSizeException()
        {
        }

        public InvalidFileSizeException(string name)
            : base(string.Format("Invalid file size of the file: {0}. The size exceeds int32 range.", name))
        {
        }

        public InvalidFileSizeException(string name, Exception inner)
            : base(string.Format("Invalid file size of the file: {0}. The size exceeds int32 range.", name), inner)
        {
        }
    }
}