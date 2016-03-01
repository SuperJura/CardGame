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
        static Dictionary<string, string> players;
        static ObservableList<string> waitingPlayers;
        string nickname;


        static LobbyBehavior()
        {
            players = new Dictionary<string, string>();
            waitingPlayers = new ObservableList<string>();
            waitingPlayers.OnAdd += WaitingPlayers_OnAdd;
        }

        private static void WaitingPlayers_OnAdd(string item)
        {
            Program.myForm.WriteToEventMsg(item + " ceka svoj match");
        }

        protected override void OnOpen()
        {
            Program.myForm.WriteToEventMsg(ID + " je otvorio konekciju");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            players.Remove(ID);
            Program.myForm.WriteToPlayerMsg(ID + " > " + nickname);
            Program.myForm.WriteToEventMsg(nickname + " se odspojio sa servera");
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

        private void WantToPlayStatement()
        {
            waitingPlayers.Add(nickname);
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
                Program.myForm.WriteToEventMsg(message[1] + " se nemoze spojiti jer je nick zauzet");
            }
        }

        private void AddedNewPlayer()
        {
            Sessions.SendTo("joined|" + nickname, ID);  //posalji klijentu poruku da se uspjesno spojio
            SendListOfPlayersToAll();   //posalji svima da se netko spojio

            Program.myForm.WriteToPlayerMsg(ID + " > " + nickname); //logiraj
            Program.myForm.WriteToEventMsg(nickname + " se spojio na server");
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
    }
}
