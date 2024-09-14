namespace ShoppingCart.SharedKernel.Exceptions;

public class EnumException : Exception
{
    public EnumException(string message) : base(message) { }

    public EnumException(string message, Exception innerException) : base(message, innerException) { }
}
