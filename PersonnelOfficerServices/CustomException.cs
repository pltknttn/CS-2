using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelOfficerServices
{
    [Serializable]
    public class CustomException: Exception
    {
        public ErrorCode ErrorCode { get; set; } 

        public CustomException() : this(null, null, ErrorCode.Fatal) { }

        public CustomException(string message) : this(message, null, ErrorCode.Fatal) { }

        public CustomException(string message, ErrorCode errorCode) : this(message, null, errorCode) { }

        public CustomException(string message, Exception innerException) : this(message, innerException, ErrorCode.Fatal) { }

        public CustomException(string message, Exception innerException, ErrorCode errorCode) : base(message, innerException) { ErrorCode = errorCode; }

        protected CustomException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }

    public enum ErrorCode
    {
        None = 0,
        Fatal = 1, 
        Warning = 2,
        InvalidInput = 3 
    }
}