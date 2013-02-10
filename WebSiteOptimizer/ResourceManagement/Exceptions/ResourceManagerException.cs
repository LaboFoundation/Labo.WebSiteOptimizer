using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class ResourceManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class.
        /// </summary>
        public ResourceManagerException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ResourceManagerException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ResourceManagerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ResourceManagerException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ResourceManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
