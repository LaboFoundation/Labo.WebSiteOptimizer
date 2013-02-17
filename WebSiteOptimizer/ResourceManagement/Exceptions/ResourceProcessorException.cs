using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class ResourceProcessorException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessorException"/> class.
        /// </summary>
        public ResourceProcessorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessorException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ResourceProcessorException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ResourceProcessorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessorException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ResourceProcessorException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProcessorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ResourceProcessorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
