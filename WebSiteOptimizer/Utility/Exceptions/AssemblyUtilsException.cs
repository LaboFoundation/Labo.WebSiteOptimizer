using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Labo.WebSiteOptimizer.Utility.Exceptions
{
    [Serializable]
    public class AssemblyUtilsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUtilsException"/> class.
        /// </summary>
        public AssemblyUtilsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUtilsException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public AssemblyUtilsException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUtilsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssemblyUtilsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUtilsException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected AssemblyUtilsException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUtilsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AssemblyUtilsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
