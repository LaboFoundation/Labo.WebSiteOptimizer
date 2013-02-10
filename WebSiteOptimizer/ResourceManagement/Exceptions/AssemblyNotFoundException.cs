using System;
using System.Runtime.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Exceptions
{
    [Serializable]
    public class AssemblyNotFoundException : ResourceManagerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNotFoundException"/> class.
        /// </summary>
        public AssemblyNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNotFoundException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public AssemblyNotFoundException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssemblyNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNotFoundException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected AssemblyNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AssemblyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
