using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class HttpResourceReaderException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResourceReaderException"/> class.
        /// </summary>
        public HttpResourceReaderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResourceReaderException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public HttpResourceReaderException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResourceReaderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public HttpResourceReaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResourceReaderException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected HttpResourceReaderException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResourceReaderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpResourceReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
