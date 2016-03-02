using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace GameServer
{
    public static class ExtensionMethodsForSession
    {
        public static void OnMatchListener(this WebSocketSessionManager session, string firstPlayer, string secondPlayer)
        {
            string firstPlayerID = LobbyBehavior.players.Where(x => x.Value == firstPlayer).FirstOrDefault().Key;
            string secondPlayerID = LobbyBehavior.players.Where(x => x.Value == secondPlayer).FirstOrDefault().Key;

            session.SendTo("playing|" + secondPlayer, firstPlayerID);
            session.SendTo("playing|" + firstPlayer, secondPlayerID);

            Program.myForm.WriteToEventMsg(firstPlayer + " i " + secondPlayer + " su zapoceli igru");
        }
    }
}
