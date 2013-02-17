using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class ResourceHandlerException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandlerException"/> class.
        /// </summary>
        public ResourceHandlerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandlerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ResourceHandlerException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandlerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ResourceHandlerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandlerException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ResourceHandlerException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHandlerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ResourceHandlerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
