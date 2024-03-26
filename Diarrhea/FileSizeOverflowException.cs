namespace Diarrhea;

/// <summary>
/// File size overflow.
/// </summary>
public class FileSizeOverflowException : Exception
{
    public FileSizeOverflowException()
    {
    }

    public FileSizeOverflowException(string name)
        : base(string.Format("Invalid file size of the file: {0}. The value exceeds int32 range.", name))
    {
    }

    public FileSizeOverflowException(string name, Exception inner)
        : base(string.Format("Invalid file size of the file: {0}. The value exceeds int32 range.", name), inner)
    {
    }
}