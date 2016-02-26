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
    class GameBehavior : WebSocketBehavior
    {
        static Dictionary<string, string> players;
        string nickname;

        static GameBehavior()
        {
            players = new Dictionary<string, string>();
        }

        protected override void OnOpen()
        {
            Program.myForm.WriteToEventMsg(ID + " je otvorio konekciju");
        }
        protected override void OnClose(CloseEventArgs e)
        {
            players.Remove(ID);
            Program.myForm.WriteToPlayerMsg(ID + " > " + nickname);
            Program.myForm.WriteToEventMsg(ID + " se odspojio sa servera");
            SendListOfPlayersToAll();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string[] message = e.Data.Split('|');
            if (message[0] == "join")
            {
                if (AddNewPlayer(message[1]))
                {
                    nickname = message[1];
                    SendListOfPlayersToAll();
                    Program.myForm.WriteToPlayerMsg(ID + " > " + nickname);
                    Program.myForm.WriteToEventMsg(ID + " se spojio na server");
                }
                else
                {
                    Sessions.SendTo("error|1", ID);
                    Program.myForm.WriteToEventMsg(message[1] + " se nemoze spojiti jer je nick zauzet");
                }
            }
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
            return true;
        }
    }
}
