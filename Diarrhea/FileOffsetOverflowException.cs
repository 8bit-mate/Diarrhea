namespace Diarrhea;

/// <summary>
/// File offset overflow.
/// </summary>
public class FileOffsetOverflowException : Exception
{
    public FileOffsetOverflowException()
    {
    }

    public FileOffsetOverflowException(string name)
        : base(string.Format("Invalid file offset of the file: {0}. The value exceeds int32 range.", name))
    {
    }

    public FileOffsetOverflowException(string name, Exception inner)
        : base(string.Format("Invalid file offset of the file: {0}. The value exceeds int32 range.", name), inner)
    {
    }
}