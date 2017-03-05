using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nepo.Common;
using Nepo.DataGenerator;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DataHandler.SaveMapConfig(Generator.GenerateInstance(new GenerationConfig()).Map);            

            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i].ToUpperInvariant())
                {
                    case "-C":
                        var count = 0;
                        try
                        {
                            count = int.Parse(args[++i]);
                        }
                        catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                        {
                            Console.WriteLine("Invalid parameter for -G, please input a number");
                            return;
                        }
                        Console.WriteLine($"Creating {count} instances");
                        GenInstancesFromCmd(count);
                        return;

                    case "-G":
                        StreamReader sr = null;
                        try
                        {
                            sr = new StreamReader(args[++i]);
                            String xmlText = sr.ReadToEnd();
                            sr.Close();
                            var foo = XmlHelper.Deserialize<IEnumerable<Guid>>(xmlText);                                                                                        
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error locating file {args[i]}: " + ex);
                        }
                        finally
                        {
                            sr?.Dispose();
                        }
                        return;                        
                }
            }
        }

        static void GenInstancesFromCmd(int count)
        {
            for (int i = 1; i <= count; ++i)
            {
                var instance = Generator.GenerateInstance(new GenerationConfig());
                Console.WriteLine($"Generated {instance.InstanceId}");
                instance.Save();
            }
        }

        static void GenInstanceFromGuids(IEnumerable<Guid> guids)
        {
            foreach (var guid in guids)
            {
                var instance = Generator.GenerateInstance(new GenerationConfig(), guid);
                Console.WriteLine($"Generated {instance.InstanceId}");
                instance.Save();
            }
        }
    }
}
