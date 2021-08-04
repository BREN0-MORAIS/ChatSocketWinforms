using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatSocket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;
        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            textInfo.Text += $"Satrting...{Environment.NewLine}";
            btnStart.Enabled = false;

            btnSend.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(textIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textInfo.Text += $"{e.IpPort} Desconectado.{Environment.NewLine}";
                listClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_DataReceived(object sender, DataReceivedFromClientEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });

        }


        private void Events_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                textInfo.Text += $"{e.IpPort} Conectado.{Environment.NewLine}";
                listClientIP.Items.Add(e.IpPort);
            });
           
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
          
            if (server.IsListening)
            {
                if(!string.IsNullOrEmpty(textMessage.Text) && listClientIP.SelectedItem != null)
                {
                    server.Send(listClientIP.SelectedItem.ToString(), textMessage.Text);
                    textInfo.Text += $"Server: {textMessage.Text}{Environment.NewLine}";
                    textMessage.Text = string.Empty;
                }
            }
        }
    }
}
