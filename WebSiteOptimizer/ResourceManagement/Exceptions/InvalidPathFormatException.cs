using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class InvalidPathFormatException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathFormatException"/> class.
        /// </summary>
        public InvalidPathFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathFormatException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public InvalidPathFormatException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathFormatException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidPathFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathFormatException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected InvalidPathFormatException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPathFormatException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidPathFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
