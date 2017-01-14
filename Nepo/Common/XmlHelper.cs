using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public class XmlHelper<T>
    {
        private static Dictionary<Type, XmlSerializer> _allSerializers = new Dictionary<Type, XmlSerializer>();
        
        private static XmlSerializer GetSerializer(Type T)
        {
            XmlSerializer serializer;
            if (!_allSerializers.TryGetValue(typeof(T), out serializer))
            {
                serializer = new XmlSerializer(typeof(T));
                _allSerializers.Add(typeof(T), serializer);
            }
            return serializer;
        }

        public static String Serialize<T>(T instance)
        {
            XmlSerializer serializer = GetSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, instance);
            return String.Empty;
        }

        public static T Deserialize<T>(String xml)
        {
            XmlSerializer serializer = GetSerializer(typeof(T));
            StringReader sr = new StringReader(xml);
            T instance = (T)(serializer.Deserialize(sr) ?? default(T));
            return instance;
        }
    }
}
