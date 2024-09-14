namespace ShoppingCart.SharedKerel.Responses;

public class SuccessResponse(object? data = null, string? message = null, int? statusCode = 200) : IResponse
{
    public bool IsSuccess { get; set; } = true;
    public int StatusCode { get; set; } = statusCode ?? 200;
    public string Message { get; set; } = message ?? "Success";
    public List<string> Errors { get; set; } = new List<string>();
    public object? Data { get; set; } = data;
}
