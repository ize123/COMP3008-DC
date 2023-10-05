using System;
using System.Runtime.Serialization;

namespace ClassLibraryDLL
{
    [Serializable]
    public class ApiException : Exception, ISerializable
    {
        public int StatusCode { get; private set; }
        public string ErrorCode { get; private set; }

        public ApiException(string message, int statusCode, string errorCode)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        // Implement the GetObjectData method for serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            // Serialize the custom properties
            info.AddValue("StatusCode", StatusCode, typeof(int));
            info.AddValue("ErrorCode", ErrorCode, typeof(string));

            // Call the base implementation to serialize the exception message and other properties
            base.GetObjectData(info, context);
        }

        // Add a constructor for deserialization
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize the custom properties
            StatusCode = (int)info.GetValue("StatusCode", typeof(int));
            ErrorCode = (string)info.GetValue("ErrorCode", typeof(string));
        }
    }

}
