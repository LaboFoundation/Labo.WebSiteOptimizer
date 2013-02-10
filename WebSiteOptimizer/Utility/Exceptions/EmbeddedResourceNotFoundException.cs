using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.Utility.Exceptions
{
    [Serializable]
    public class EmbeddedResourceNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceNotFoundException"/> class.
        /// </summary>
        public EmbeddedResourceNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public EmbeddedResourceNotFoundException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EmbeddedResourceNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected EmbeddedResourceNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmbeddedResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
