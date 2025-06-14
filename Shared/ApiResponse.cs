namespace BlogPortal.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Meta { get; set; }

        public ApiResponse(bool success, string message, T? data = default, object? meta = default)
        {
            Success = success;
            Message = message;
            Data = data;
            Meta = meta;
        }
    }
}