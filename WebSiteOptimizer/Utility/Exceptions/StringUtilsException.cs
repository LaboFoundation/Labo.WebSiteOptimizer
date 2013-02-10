using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.Utility.Exceptions
{
    [Serializable]
    public class StringUtilsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringUtilsException"/> class.
        /// </summary>
        public StringUtilsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringUtilsException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public StringUtilsException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringUtilsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public StringUtilsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringUtilsException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected StringUtilsException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringUtilsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public StringUtilsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
