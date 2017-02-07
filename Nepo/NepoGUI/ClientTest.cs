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

            List<Tuple<int, bool>> votes = new List<Tuple<int, bool>>();
            votes.Add(new Tuple<int, bool>(0, true));
            votes.Add(new Tuple<int, bool>(1, false));
            votes.Add(new Tuple<int, bool>(2, false));

            client.Vote(votes);
            Console.Write("");

        }

        private static void Client_NewDataAvailable(object sender, NewDataEventArgs e)
        {
            List<Solution> list = e.ProposedSolutions;
            Console.WriteLine("");
        }
    }
}
