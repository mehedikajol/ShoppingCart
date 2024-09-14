namespace ShoppingCart.SharedKerel.Responses;

public class ErrorResponse(string message, int statusCode = 500) : IResponse
{
    public bool IsSuccess { get; set; } = false;
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public List<string> Errors { get; set; } = new List<string>();
    public object? Data { get; set; } = null;
}
