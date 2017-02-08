using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nepo.Common
{

    public class Logger
    {


        public static void Dumpvalue(String value)
        {
            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            using (StreamWriter file =
                    new StreamWriter(@"C:\tmp\sample.txt"))
            {
                file.WriteLine(value);
            }

        }


    }





}
