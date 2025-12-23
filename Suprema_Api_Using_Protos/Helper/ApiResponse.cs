namespace Suprema_Api_Using_Protos.Helper
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = new object();

        public ApiResponse() { }

        public ApiResponse(T? data = default, bool success = true, string message = "" )
        {
            Success = success;
            Message = message;
            if (data == null)
            {
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    Data = Activator.CreateInstance(typeof(T))!;
                }
                else if (typeof(T).IsArray)
                {
                    Data = Array.CreateInstance(typeof(T).GetElementType()!, 0);
                }
                else
                {
                    Data = new object();
                }
            }
            else
            {
                Data = data!;
            }

        }
    }
}