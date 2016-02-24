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
        protected override void OnOpen()
        {
            Program.myForm.WriteToEventMsg("JOINED: " + ID);
            Program.myForm.WriteToPlayerMsg("JOINED: " + ID);
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Program.myForm.WriteToPlayerMsg(ID);
        }
    }
}
