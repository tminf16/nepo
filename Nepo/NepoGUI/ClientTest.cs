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
        private static List<Solution> solution = new List<Solution>();
        private static Instance instance;
        private static NepoClient client;

        static void Main(string[] args)
        {

            System.Console.WriteLine("Client Test");

            NepoClient _client = new NepoClient();
            client = _client;
            _client.NewDataAvailable += Client_NewDataAvailable;

            // Register on Mediator Server
            Instance _instance = _client.Register();
            instance = _instance;

            nextRound();

            while (true) ;
        }


        private static void nextRound()
        {
            client.Vote(ValidateOffer(instance));
        }



        private static List<Tuple<int, bool>> ValidateOffer(Instance instance)
        {
            List<Tuple<int, bool>> votes = new List<Tuple<int, bool>>();

            // Zugriff auch auf "List<Solution> solution"

            // MOCK
            votes.Add(new Tuple<int, bool>(0, true));
            votes.Add(new Tuple<int, bool>(1, false));
            votes.Add(new Tuple<int, bool>(2, false));

            return votes;
        }


        private static void Client_NewDataAvailable(object sender, NewDataEventArgs e)
        {
            List<Solution> list = e.ProposedSolutions;
            solution = list;

            nextRound();
        }
    }
}
