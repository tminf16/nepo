using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nepo.Common;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    class ClientTest
    {


        static void Main(string[] args)
        {

            System.Console.WriteLine("Client Test");

            NepoClient client = new NepoClient();
            Instance instance = client.Register();
            client.NewDataAvailable += Client_NewDataAvailable;
            Console.Write("");


        }

        private static void Client_NewDataAvailable(object sender, NewDataEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
