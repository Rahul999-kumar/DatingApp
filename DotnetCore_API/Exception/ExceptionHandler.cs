namespace DotnetCore_API.Exception
{
    public class ExceptionHandler
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ExceptionHandler(int statusCode,string message=null,string details=null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
