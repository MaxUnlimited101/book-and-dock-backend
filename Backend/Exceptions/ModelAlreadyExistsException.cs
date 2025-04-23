namespace Backend.Exceptions;

public class ModelAlreadyExistsException : Exception
{
    public ModelAlreadyExistsException(string message) : base(message) {}
    public ModelAlreadyExistsException() {}
}