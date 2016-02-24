using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp.Server;

namespace GameServer
{
    public partial class MainForm : Form
    {
        private WebSocketServer wsServer;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            wsServer = new WebSocketServer(8080);
            wsServer.AddWebSocketService<GameBehavior>("/GameBehavior");
            wsServer.Start();
            WriteToEventMsg("Server started on port 8080");
        }

        public void WriteToEventMsg(string msg)
        {
            Invoke((Action)(() => WriteToLogMsgToListbox(msg)));
        }

        private void WriteToLogMsgToListbox(string msg)
        {
            lbEvents.Items.Add(msg);
        }

        public void WriteToPlayerMsg(string ID)
        {
            Invoke((Action)(() => WriteToPlayerListbox(ID)));
        }

        private void WriteToPlayerListbox(string ID)
        {
            if (lbPlayers.Items.Contains(ID))
            {
                lbPlayers.Items.Remove(ID);
            }
            else
            {
                lbPlayers.Items.Add(ID);
            }
        }
    }
}
