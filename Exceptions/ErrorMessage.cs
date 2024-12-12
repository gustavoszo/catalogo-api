using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatalogoApi.Exceptions
{
    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }

        public ErrorMessage(int statusCode, string message, string path)
        {
            StatusCode = statusCode;
            Message = message;
            Path = path;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
