using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teamspeak.Sdk
{
    /// <summary>
    /// The exception that is thrown when a call to the native library failed.
    /// </summary>
    [Serializable]
    public class TeamspeakException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamspeakException"/> class with a specified error code and error message.
        /// </summary>
        /// <param name="errorCode">A error code that describes the error.</param>
        /// <param name="message">A string that describes the error. The content of message is intended to be understood by humans.</param>
        public TeamspeakException(Error errorCode, string message) : this(errorCode, message, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamspeakException"/> class with a specified error code, error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="errorCode">A error code that describes the error.</param>
        /// <param name="message">A string that describes the error. The content of message is intended to be understood by humans.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public TeamspeakException(Error errorCode, string message, Exception inner) : base(message, inner) 
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamspeakException"/> with the specified serialization and context information.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected TeamspeakException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        {
            ErrorCode = (Error)info.GetInt32("TeamspeakErrorCode");
        }

        /// <summary>
        /// Error code return by the native library.
        /// </summary>
        public Error ErrorCode { get; set; }

        /// <summary>
        /// Sets the SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("TeamspeakErrorCode", (Int32)ErrorCode);
            base.GetObjectData(info, context);
        }
    }
}
