namespace HotPizza.Services;

public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
