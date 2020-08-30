using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace coreMongo.Model
{
    public class ErrorResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        public ErrorResponse(string errorCode, string errorMessage)
        {
            Code = errorCode;
            Message = errorMessage;
        }
    }
}
