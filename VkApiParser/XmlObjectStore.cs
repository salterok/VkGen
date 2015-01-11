using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace VkApiParser
{
    /// <summary>
    /// Serialize/Deserialize object
    /// </summary>
    /// <remarks>No instance - do not cache serialized type</remarks>
    public static class XmlObjectStore
    {
        public static void Store<T>(T obj, string filename) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.CreateText(filename))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public static T Retrive<T>(string filename) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = File.OpenText(filename))
            {
                return serializer.Deserialize(stream) as T;
            }
        }
    }
}
