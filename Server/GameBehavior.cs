using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameServer
{
    class GameBehavior : WebSocketBehavior
    {

        public static Dictionary<string, string> playersInGames;    //nickname, nickname
        public static Dictionary<string, string> players;   //ID, nickname
        private static object syncLockStartGame;
        private string nickname;
        private string opponentNickname;

        static GameBehavior()
        {
            playersInGames = new Dictionary<string, string>();
            players = new Dictionary<string, string>();
            syncLockStartGame = new object();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string[] message = e.Data.Split('|');
            switch (message[0])
            {
                case "startGame":
                    StartGameStatement(message[1]);
                    break;
                case "cardDrawed":
                    CardDrawedStatement(message[1]);
                    break;
                case "cardPlayed":
                    CardPlayedStatement(message[1]);
                    break;
            }
        }

        private void CardPlayedStatement(string staticID)
        {
            Sessions.SendTo("opponentPlayed|" + staticID, GetOpponentID());
        }

        private void CardDrawedStatement(string staticID)
        {
            Sessions.SendTo("opponentDrawed|" + staticID, GetOpponentID());
        }

        private void StartGameStatement(string nicknamesInMessage)
        {
            string[] nicknames = nicknamesInMessage.Split(';');
            nickname = nicknames[0];
            opponentNickname = nicknames[1];

            players.Add(ID, nickname);
            if (GetOpponentID() != "")
            {
                Thread.Sleep(100);
            }
            //samo jedan igrac moze "zapoceti" igru, tj. upisati sebe i protivnika u listu igraca
            lock (syncLockStartGame)
            {
                //prvi thread(player) ce uci u if, drugi nece
                if (!playersInGames.ContainsValue(nickname))
                {
                    playersInGames.Add(nickname, opponentNickname);
                    Logger.LogEventMsg(nickname + " " + opponentNickname + " su poceli igrati!");
                    Logger.LogPlayingPlayers(playersInGames.Count * 2);
                    Logger.LogEventMsg("OpponentID: " + GetOpponentID());
                    Sessions.SendTo("playerOnTurn|" + nickname, GetOpponentID());
                }
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            string otherPlayerID = GetOpponentID();
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

        private string GetOpponentID()
        {
            return players.Where(x => x.Value == opponentNickname).FirstOrDefault().Key;
        }
    }

}
