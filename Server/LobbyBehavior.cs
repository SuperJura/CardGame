using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

//error codes:
//1 - nick je zauzet

namespace GameServer
{
    class LobbyBehavior : WebSocketBehavior
    {
        public static Dictionary<string, string> players;  // ID, nickname
        Matchmaking matchmaking;
        static Random r;
        string nickname;

        static LobbyBehavior()
        {
            r = new Random();
            players = new Dictionary<string, string>();
        }

        public LobbyBehavior()
        {
            matchmaking = Matchmaking.GetInstance();
        }

        protected override void OnOpen()
        {
            Logger.LogOnOpen(ID);
            matchmaking.OnMatch += Sessions.OnMatchListener;    //posto postoji samo jedan sessionManager, samo jedan objekt ce biti predplacen na OnMatch
                                                                //sessionManager se inicializira pri prvom requestu, tako da nemoze biti u konstruktoru
        }

        protected override void OnClose(CloseEventArgs e)
        {
            if (matchmaking.Contains(players[ID]))    //ako je cekao match, makni ga
            {
                matchmaking.Remove(players[ID]);
            }

            players.Remove(ID);
            Logger.LogAllPlayersInLobby(players.Count);
            Logger.LogOnlinePlayer(nickname);
            Logger.LogEventMsg(nickname + " se odspojio sa servera");
            SendListOfPlayersToAll();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string[] message = e.Data.Split('|');
            switch (message[0])
            {
                case "join":
                    JoinStatement(message);
                    break;
                case "wantToPlay":
                    WantToPlayStatement();
                    break;
            }
        }

        private void JoinStatement(string[] message)
        {
            if (AddNewPlayer(message[1]))
            {
                AddedNewPlayer();
            }
            else
            {
                Sessions.SendTo("error|1", ID);
                Logger.LogEventMsg(message[1] + " se nemoze spojiti jer je nick zauzet");
            }
        }

        private void WantToPlayStatement()
        {
            Logger.LogEventMsg(nickname + " ceka za match");
            matchmaking.Add(nickname);
        }

        private bool AddNewPlayer(string nick)
        {
            if (players.Values.Contains(nick))
            {
                return false;   //ako vec postoji igrac s tim nickom, vrati false
            }
            players.Add(ID, nick);
            nickname = nick;    //postavi nick u klasnu varijablu
            return true;
        }

        private void AddedNewPlayer()
        {
            Sessions.SendTo("joined|" + nickname, ID);  //posalji klijentu poruku da se uspjesno spojio
            SendListOfPlayersToAll();   //posalji svima da se netko spojio

            Logger.LogOnlinePlayer(nickname); //logiraj
            Logger.LogEventMsg(nickname + " se spojio na server");
            Logger.LogAllPlayersInLobby(players.Count);
        }

        private void SendListOfPlayersToAll()
        {
            StringBuilder sb = new StringBuilder("list|");
            foreach (string nick in players.Values)
            {
                sb.Append(nick + ";");
            }
            sb.Remove(sb.Length - 1, 1);
            foreach (string id in players.Keys)
            {
                Sessions.SendTo(sb.ToString(), id);
            }
        }

    }
}
