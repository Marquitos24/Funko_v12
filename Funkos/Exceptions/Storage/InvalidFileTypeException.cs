namespace Funkos.Exceptions;

public class InvalidFileTypeException : Exception
{
    public string? FileType { get; }
    public string[]? AllowedTypes { get; }

    public InvalidFileTypeException(string message) : base(message) { }
}