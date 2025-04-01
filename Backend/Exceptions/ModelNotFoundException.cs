namespace Backend.Exceptions;

public class ModelNotFoundException : Exception
{
    public ModelNotFoundException(string message) : base(message) {}
    public ModelNotFoundException() {}
}