namespace Backend.Exceptions;

/// <summary>
/// Exception thrown when a model is invalid.
/// This can be due to various reasons such as missing required fields,
/// incorrect data types, or any other validation errors.
/// </summary>
public class ModelInvalidException : Exception
{
    public ModelInvalidException(string message) : base(message) {}
    public ModelInvalidException() {}
}