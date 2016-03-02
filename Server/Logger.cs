using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class Logger
    {
        public static void LogOnOpen(string ID)
        {
            Program.myForm.WriteToEventMsg(ID + " je otvorio konekciju");
        }

        public static void LogAllPlayerCounter(int count)
        {
            Program.myForm.WriteToAllPlayerCounter(count);
        }

        public static void LogOnlinePlayer(string nickname)
        {
            Program.myForm.WriteToPlayerMsg(nickname);
        }

        public static void LogEventMsg(string msg)
        {
            Program.myForm.WriteToEventMsg(msg);
        }

        public static void LogWaitingPlayers(int count)
        {
            Program.myForm.WriteToAllPlayerWaitingCounter(count);
        }
    }
}
