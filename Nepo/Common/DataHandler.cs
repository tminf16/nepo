using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class DataHandler
    {
        private static T GetConfig<T>()
        {
            String filename = typeof(T).Name + ".xml";
            T config = default(T);
            if (!File.Exists(filename))
                File.Create(filename).Close();
                
            using (StreamReader sr = new StreamReader(filename))
            { 
                String xmlText = sr.ReadToEnd();
                sr.Close();

                config = XmlHelper.Deserialize<T>(xmlText);

                if (null == config)
                {
                    config = (T)Activator.CreateInstance(typeof(T), new object[] { });
                    config.Save();
                }
            }
            return config;
        }

        public static void SaveMapConfig(MapConfig config)
        {
            config.Save();
        }

        public static void SaveAgentConfig(AgentConfig config)
        {
            config.Save();
        }
        private static MapConfig _map = null;
        public static MapConfig GetMapConfig()
        {
            return _map ?? (_map = GetConfig<MapConfig>());
        }

        private static AgentConfig _agent = null;
        public static AgentConfig GetAgentConfig()
        {
            return _agent ?? (_agent = GetConfig<AgentConfig>());
        }
    }

    public static class XmlExtensions
    {
        public static void Save<T>(this T instance)
        {
            String filename = String.Empty;
            if (typeof(MapConfig) == typeof(T) || typeof(AgentConfig) == typeof(T))
                filename = typeof(T).Name + ".xml";
            if (String.IsNullOrEmpty(filename))
                return;
            
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(XmlHelper.Serialize(instance));
            sw.Close();
        }
    }
}
