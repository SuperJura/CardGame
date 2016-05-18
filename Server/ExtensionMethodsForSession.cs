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
            string firstPlayerID = LobbyBehavior.players.FirstOrDefault(x => x.Value == firstPlayer).Key;
            string secondPlayerID = LobbyBehavior.players.FirstOrDefault(x => x.Value == secondPlayer).Key;

            session.SendTo("playing|" + secondPlayer, firstPlayerID);
            session.SendTo("playing|" + firstPlayer, secondPlayerID);

            Logger.LogEventMsg(firstPlayer + " i " + secondPlayer + " su zapoceli igru");
        }
    }
}
