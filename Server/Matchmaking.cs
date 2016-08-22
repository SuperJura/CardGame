using System;
using System.Collections.Generic;
using System.Data;
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

        public void Add(string player, WebSocketSessionManager session)
        {
            Add(player);
            Logger.LogWaitingPlayers(Count);
            if (Count >= 2)
            {
                string otherPlayer = this.Where(x => x != player).ToList()[r.Next(0, Count - 1)];
                Match(player, otherPlayer, session);
                Remove(player);
                Remove(otherPlayer);
            }
        }

        private void Match(string firstPlayer, string secondPlayer, WebSocketSessionManager session)
        {
            string firstPlayerID = GetPlayerID(firstPlayer);
            string secondPlayerID = GetPlayerID(secondPlayer);

            session.SendTo("playing|" + secondPlayer, firstPlayerID);
            session.SendTo("playing|" + firstPlayer, secondPlayerID);

            Logger.LogEventMsg(firstPlayer + " i " + secondPlayer + " su zapoceli igru");
        }

        public new void Remove(string player)
        {
            base.Remove(player);
            Logger.LogWaitingPlayers(Count);
        }

        private static string GetPlayerID(string playerName)
        {
            return LobbyBehavior.players.FirstOrDefault(x => x.Value == playerName).Key;
        }
    }
}
