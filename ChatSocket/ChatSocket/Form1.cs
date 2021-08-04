﻿using SimpleTcp;
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
        SimpleTcpClient client;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try 
            {
                client.Connect();

                btnSend.Enabled = true;
                btnConnect.Enabled = false;
            
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Menssagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new(textIP.Text);
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
            btnSend.Enabled = false;

        }

        private void Events_Disconnected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textInfo.Text += $"Server disconnected.{Environment.NewLine}";
            });
        }

        private void Events_DataReceived(object sender, DataReceivedFromServerEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textInfo.Text += $"Server:{Encoding.UTF8.GetString(e.Data)} {Environment.NewLine}";
            });

        }

        private void Events_Connected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textInfo.Text += $"Server Conectado.{Environment.NewLine}";
            });

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                if(! string.IsNullOrEmpty(textMessage.Text))
                {
                    client.Send(textMessage.Text);
                    textInfo.Text += $"Me:{textMessage.Text}{Environment.NewLine}";
                    textMessage.Text = string.Empty;
                }
            }
        }
    }
}
