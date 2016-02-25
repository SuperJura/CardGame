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
        static Dictionary<string, string> players;

        public GameBehavior()
        {
            players = new Dictionary<string, string>();
        }

        protected override void OnOpen()
        {
            Program.myForm.WriteToEventMsg("JOINED: " + ID);
            Program.myForm.WriteToPlayerMsg(ID);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Program.myForm.WriteToPlayerMsg(ID);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string[] message = e.Data.Split('|');
            if (message[0] == "join")
            {
                players.Add(ID, message[1]);
                Sessions.Broadcast("joined|" + message[1]);
            }
        }
    }
}
