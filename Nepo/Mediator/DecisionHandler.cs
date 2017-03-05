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
        public int CurrentRound { get { return current_round; } }
        private Dictionary<Tuple<int, Guid>, List<Tuple<int, bool>>> Liste = new Dictionary<Tuple<int, Guid>, List<Tuple<int, bool>>>();

        private Dictionary<Guid, List<int>> votesForCurrentRound = new Dictionary<Guid, List<int>>();
       

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
            votesForCurrentRound[guidOfClient] = decision.Where(x => x.Item2).Select(x => x.Item1).ToList();
            // Client hat in dieser runde noch nicht abgestimmt
            if (hasClientVoted(guidOfClient) == false)
            {
                Liste.Add(round, decision);
            }

        }

        public List<Tuple<Guid, int>> GetVotesForRound()
        {
            var list = votesForCurrentRound.Select(x => x.Value.Select(y => new Tuple<Guid, int>(x.Key, y))).ToList();
            var result = list.SelectMany(x => x).ToList();
            return result;
        } 


        public void newRound()
        {
            current_round++;
            votesForCurrentRound = new Dictionary<Guid, List<int>>();
        }

        public void Reset()
        {
            current_round = 0;
            votesForCurrentRound = new Dictionary<Guid, List<int>>();
            Liste = new Dictionary<Tuple<int, Guid>, List<Tuple<int, bool>>>();
        }

    }
}