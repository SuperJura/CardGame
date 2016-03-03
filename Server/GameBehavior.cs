using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameServer
{
    class GameBehavior : WebSocketBehavior
    {

        public static Dictionary<string, string> playersInGames;    //nickname, nickname
        public static Dictionary<string, string> players;   //ID, nickname
        private static object syncLock;
        private string nickname;
        private string opponentNickname;

        static GameBehavior()
        {
            playersInGames = new Dictionary<string, string>();
            players = new Dictionary<string, string>();
            syncLock = new object();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string[] message = e.Data.Split('|');
            switch (message[0])
            {
                case "startGame":
                    StartGameStatement(message[1]);
                    break;
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            string otherPlayerID = players.Where(x => x.Value == opponentNickname).FirstOrDefault().Key;
            if (playersInGames.Keys.Contains(nickname))
            {
                players.Remove(ID);
                playersInGames.Remove(nickname);
            }
            else
            {
                players.Remove(otherPlayerID);
                playersInGames.Remove(opponentNickname);
            }

            Logger.LogPlayingPlayers(playersInGames.Count * 2);
            Sessions.SendTo("unexpectedEnd|", otherPlayerID);
        }

        private void StartGameStatement(string nicknamesInMessage)
        {
            lock (syncLock)
            {
                string[] nicknames = nicknamesInMessage.Split(';');
                nickname = nicknames[0];
                opponentNickname = nicknames[1];

                players.Add(ID, nickname);

                if (!playersInGames.ContainsValue(nickname))
                {
                    playersInGames.Add(nickname, opponentNickname);
                    Logger.LogEventMsg(nickname + " " + opponentNickname + " su poceli igrati!");
                    Logger.LogPlayingPlayers(playersInGames.Count * 2);
                }

            }
        }
    }

}
