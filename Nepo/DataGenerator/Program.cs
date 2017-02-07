using System;
using System.Collections.Generic;
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
            DataHandler.SaveMapConfig(Generator.GenerateInstance(new MapGenerationConstraints()).Map);
        }
    }
}
