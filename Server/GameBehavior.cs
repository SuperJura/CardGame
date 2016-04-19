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
        private static int playersCount;

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
            Logger.LogEventMsg(nickname + " je odigrao" + staticID);
            Sessions.SendTo("opponentPlayed|" + staticID, GetOpponentID());
        }

        private void CardDrawedStatement(string staticID)
        {
            Logger.LogEventMsg(nickname + " je povukao " + staticID + " saljem " + GetOpponentID());
            Sessions.SendTo("opponentDrawed|" + staticID, GetOpponentID());
        }

        private void StartGameStatement(string nicknamesInMessage)
        {
            string[] nicknames = nicknamesInMessage.Split(';');
            nickname = nicknames[0];
            opponentNickname = nicknames[1];

            players.Add(ID, nickname);
            while(GetOpponentID() == "")
            {
                Thread.Sleep(100);
            }
            Logger.LogEventMsg(nickname + " igra protiv:" + opponentNickname);
            //samo jedan igrac moze "zapoceti" igru, tj. upisati sebe i protivnika u listu igraca
            lock (syncLockStartGame)
            {
                //prvi thread(player) ce uci u if, drugi nece
                if (!playersInGames.ContainsValue(nickname))
                {
                    playersInGames.Add(nickname, opponentNickname);
                }
            }
            playersCount++;
            if (playersCount %2 == 0)
            {
                Sessions.SendTo("canStart|", ID);
                Sessions.SendTo("canStart|", GetOpponentID());
                Sessions.SendTo("playerOnTurn|" + nickname, GetOpponentID());
            }
            Logger.LogPlayingPlayers(playersCount);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            players.Remove(ID);
            string otherPlayerId = GetOpponentID();
            if (playersInGames.Keys.Contains(nickname))
            {
                playersInGames.Remove(nickname);
            }
            else
            {
                players.Remove(otherPlayerId);
                playersInGames.Remove(opponentNickname);
            }

            Logger.LogPlayingPlayers(playersInGames.Count * 2);
            Sessions.SendTo("unexpectedEnd|", otherPlayerId);
        }

        private string GetOpponentID()
        {
            return players.FirstOrDefault(x => x.Value == opponentNickname).Key;
        }
    }

}
