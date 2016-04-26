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

        public static List<string> playersInGames;    //nickname
        public static Dictionary<string, string> players;   //ID, nickname
        private static readonly object syncLockStartGame;
        private string nickname;
        private string opponentNickname;

        static GameBehavior()
        {
            playersInGames = new List<string>();
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
            Logger.LogEventMsg(nickname + " igra protiv:" + opponentNickname);
            //samo jedan igrac moze "zapoceti" igru, tj. upisati sebe i protivnika u listu igraca
            lock (syncLockStartGame)
            {
                playersInGames.Add(nickname);
                if (playersInGames.Contains(opponentNickname))
                {
                    Sessions.SendTo("canStart|", ID);
                    Sessions.SendTo("canStart|", GetOpponentID());
                    Sessions.SendTo("playerOnTurn|" + nickname, GetOpponentID());
                }
            }
            Logger.LogPlayingPlayers(playersInGames.Count);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            players.Remove(ID);
            string otherPlayerId = GetOpponentID();
            if (playersInGames.Contains(nickname))
            {
                playersInGames.Remove(nickname);
            }
            if (playersInGames.Contains(opponentNickname))
            {
                playersInGames.Remove(opponentNickname);
            }

            Logger.LogPlayingPlayers(playersInGames.Count);
            Sessions.SendTo("unexpectedEnd|", otherPlayerId);
        }

        private string GetOpponentID()
        {
            return players.FirstOrDefault(x => x.Value == opponentNickname).Key;
        }
    }

}
