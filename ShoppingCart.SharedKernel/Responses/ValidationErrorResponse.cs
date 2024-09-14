namespace ShoppingCart.SharedKerel.Responses;

public class ValidationErrorResponse(List<string> errors) : IResponse
{
    public bool IsSuccess { get; set; } = false;
    public int StatusCode { get; set; } = 400;
    public string Message { get; set; } = "One or more validation errors occurred.";
    public List<string> Errors { get; set; } = errors;
    public object? Data { get; set; } = null;
}
