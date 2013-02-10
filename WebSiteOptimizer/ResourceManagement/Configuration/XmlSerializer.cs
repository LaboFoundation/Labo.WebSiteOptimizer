using System;
using System.IO;
using System.Xml;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class XmlSerializer
    {
        /// <summary>
        /// Serializes the specified @object.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <returns></returns>
        public string Serialize(object @object)
        {
            Type objectType = @object.GetType();
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(objectType);
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, @object);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Deserializes the specified XML text.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public object Deserialize(string xmlText, Type objectType)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(objectType);
            using (TextReader textReader = new StringReader(xmlText))
            {
                return xmlSerializer.Deserialize(textReader);
            }
        }

        /// <summary>
        /// Deserializes the specified XML text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText">The XML text.</param>
        /// <returns></returns>
        public T Deserialize<T>(string xmlText)
        {
            return (T)Deserialize(xmlText, typeof(T));
        }
    }
}