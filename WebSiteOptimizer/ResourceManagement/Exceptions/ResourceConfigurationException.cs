using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class ResourceConfigurationException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationException"/> class.
        /// </summary>
        public ResourceConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ResourceConfigurationException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ResourceConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected ResourceConfigurationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ResourceConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
