using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediator
{

    // Struktur mit (Rundennummer, Client, Abstimmergebnis)
    public class DecisionHandler
    {
        private int current_round = 0;
        private Dictionary<Tuple<int, Guid>, List<Tuple<int, bool>>> Liste = new Dictionary<Tuple<int, Guid>, List<Tuple<int, bool>>>();

        /// <summary>
        /// Check if Client with given GUID has voted in current round
        /// </summary>
        /// <param name="guidOfClient"></param>
        /// <returns></returns>
        public bool hasClientVoted(Guid guidOfClient)
        {
            return Liste.ContainsKey(new Tuple<int, Guid>(current_round, guidOfClient));
        }

        public void saveVote(Guid guidOfClient, List<Tuple<int, bool>> decision)
        {
            Tuple<int, Guid> round = new Tuple<int, Guid>(current_round, guidOfClient);

            // Client hat in dieser runde noch nicht abgestimmt
            if (hasClientVoted(guidOfClient) == false)
            {
                Liste.Add(round, decision);
            }

        }


        public void newRound()
        {
            current_round++;
        }

    }
}