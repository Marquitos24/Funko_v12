namespace Funkos.Exceptions;

public class FileSizeExceededException : Exception
{
    public long MaxSize { get; }
    public long ActualSize { get; }

    public FileSizeExceededException(string message) : base(message) { }

    public FileSizeExceededException(string message, long maxSize, long actualSize)
        : base(message)
    {
        MaxSize = maxSize;
        ActualSize = actualSize;
    }
}

