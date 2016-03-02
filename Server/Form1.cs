using System;
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
            wsServer.AddWebSocketService<LobbyBehavior>("/LobbyBehavior");
            wsServer.Start();
            WriteToEventMsg("Server slusa na portu 8080");
            btnStartServer.Enabled = false;
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

        public void WriteToAllPlayerCounter(int count)
        {
            Invoke((Action)(() => WriteToTxtUkupnoIgraca(count)));
        }

        public void WriteToTxtUkupnoIgraca(int count)
        {
            txtUkupnoIgraca.Text = count.ToString();
        }

        public void WriteToAllPlayerWaitingCounter(int counter)
        {
            Invoke((Action)(() => WriteToTxtUkupnoIgracaCekaju(counter)));
        }

        public void WriteToTxtUkupnoIgracaCekaju(int counter)
        {
            txtUkupnoIgracaCekaju.Text = counter.ToString();
        }
    }
}
