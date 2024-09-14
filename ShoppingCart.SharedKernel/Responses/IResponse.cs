namespace ShoppingCart.SharedKerel.Responses;

public interface IResponse
{
    bool IsSuccess { get; }
    int StatusCode { get; }
    string Message { get; }
    List<string> Errors { get; }
    object? Data { get; }
}
