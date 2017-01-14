using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nepo.Common
{
    public class XmlHelper
    {
        private static Dictionary<Type, XmlSerializer> _allSerializers = new Dictionary<Type, XmlSerializer>();
        
        private static XmlSerializer GetSerializer<T>()
        {
            if (!_allSerializers.TryGetValue(typeof(T), out var serializer))
            {
                serializer = new XmlSerializer(typeof(T));
                _allSerializers.Add(typeof(T), serializer);
            }
            return serializer;
        }

        public static String Serialize<T>(T instance)
        {
            XmlSerializer serializer = GetSerializer<T>();
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, instance);
            return sw.ToString();
        }

        public static T Deserialize<T>(String xml)
        {
            XmlSerializer serializer = GetSerializer<T>();
            StringReader sr = new StringReader(xml);
            T instance = default(T);
            try
            {
                instance = (T)serializer.Deserialize(sr);
            }
            catch (Exception){}
            
            return instance;
        }
    }
}
