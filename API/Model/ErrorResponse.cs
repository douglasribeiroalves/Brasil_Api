using System.Net;

namespace API.Model
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public ErrorResponse(HttpStatusCode statusCode, string message, string type, string name)
        {
            StatusCode = statusCode;
            Message = message;
            Type = type;
            Name = name;
        }
    }
}
