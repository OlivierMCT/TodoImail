using System.Runtime.Serialization;

namespace TodoImail.Services.Models {
    public class TodoImailServiceException : Exception {
        public int Code { get; }

        public TodoImailServiceException(int code) : this(code, null!, null!) {}
        public TodoImailServiceException(int code, string? message) : this(code, message, null!) {}
        public TodoImailServiceException(int code, string? message, Exception? innerException) : base(message, innerException) {
            Code = code;
        }
    }
}
