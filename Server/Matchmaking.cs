using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameServer
{
    class Matchmaking : List<string>
    {
        private static Matchmaking instance;
        private static Random r;

        public delegate void OnMatchHandler(string firstPlayer, string secondPlayer);
        private OnMatchHandler onMatch;
        public event OnMatchHandler OnMatch
        {
            add
            {
                if (onMatch == null)
                {
                    onMatch += value;
                }
            }
            remove
            {
                onMatch -= value;
            }
        }

        private Matchmaking() { }

        public static Matchmaking GetInstance()
        {
            if (instance == null)
            {
                instance = new Matchmaking();
                r = new Random();
            }
            return instance;
        }

        public new void Add(string player)
        {
            base.Add(player);
            Logger.LogWaitingPlayers(Count);
            if (Count >= 2)
            {
                string otherPlayer = this.Where(x => x != player).ToList()[r.Next(0, Count - 1)];
                onMatch(player, otherPlayer);
                Remove(player);
                Remove(otherPlayer);
            }
        }

        public new void Remove(string player)
        {
            base.Remove(player);
            Logger.LogWaitingPlayers(Count);
        }
    }
}
