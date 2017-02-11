using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{
    public class DataHandler
    {
        private static T GetConfig<T>(String instanceFolder = null)
        {
            String filename = typeof(T).Name + ".xml";
            filename = Directory.GetCurrentDirectory() + "\\" + (null == instanceFolder ? "" : (instanceFolder + "\\")) + filename;
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

        public static void SaveMapConfig(MapConfig config, string instance = null)
        {
            config.Save(instancename: instance);
        }

        public static void SaveAgentConfig(AgentConfig config, string instance = null)
        {
            config.Save(instancename: instance);
        }


        public static void SaveAgentConfigs(List<AgentConfig> configs, string instance = null)
        {
            configs.Save(instancename: instance);
        }
        private static MapConfig _map = null;
        public static MapConfig GetMapConfig(string instance = null)
        {
            if(String.IsNullOrEmpty(instance))
                return _map ?? (_map = GetConfig<MapConfig>());
            return GetConfig<MapConfig>(instance);
        }

        private static AgentConfig _agent = null;
        public static AgentConfig GetAgentConfig(string instance = null)
        {
            if (String.IsNullOrEmpty(instance))
                return _agent ?? (_agent = GetConfig<AgentConfig>());
            return GetConfig<AgentConfig>(instance);
        }


        private static int _forcedAcceptance = 0;
        public static int GetForcedAcceptance(string instance = null)
        {
            if (String.IsNullOrEmpty(instance))
                return _forcedAcceptance = GetConfig<int>();
            return GetConfig<int>(instance);
        }



        public static List<AgentConfig> GetAgentConfigs(string instance)
        {
            return GetConfig<List<AgentConfig>>(instance);
        }
    }

    public static class XmlExtensions
    {
        public static void Save<T>(this T instance, string filename = null, string instancename = null)
        {

            if (string.IsNullOrEmpty(filename) && (typeof(MapConfig) == typeof(T) || typeof(AgentConfig) == typeof(T) || typeof(List<AgentConfig>) == typeof(T)))
            {
                filename = typeof(T).Name + ".xml";
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            filename = Directory.GetCurrentDirectory() + "\\" + (null == instancename ? "" : (instancename + "\\")) + filename;
            StreamWriter sw = new StreamWriter(filename);

            sw.Write(XmlHelper.Serialize(instance));            
            sw.Close();
        }
    }
}
