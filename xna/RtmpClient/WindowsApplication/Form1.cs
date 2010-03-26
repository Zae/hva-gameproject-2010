using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FluorineFx;
using FluorineFx.Net;
using FluorineFx.Messaging;
using FluorineFx.Messaging.Rtmp;
using FluorineFx.Messaging.Api;
using FluorineFx.Messaging.Api.Service;

namespace WindowsApplication
{
    public partial class Form1 : Form, IPendingServiceCallback
    {
        NetConnection _netConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create RTMP client
            _netConnection = new NetConnection();
            _netConnection.OnConnect += new ConnectHandler(_netConnection_OnConnect);
            _netConnection.NetStatus += new NetStatusHandler(_netConnection_NetStatus);
            UpdateState(false);
        }

        void _netConnection_OnConnect(object sender, EventArgs e)
        {
            UpdateState(true);
        }

        void _netConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            string level = e.Info["level"] as string;
            if (level == "error")
            {
                //received an error
                LogText("Error: " + e.Info["code"] as string + "\r\n");
            }
            if (level == "status")
            {
                LogText("Status: " + e.Info["code"] as string + "\r\n");
            }
        }

        private void _buttonConnect_Click(object sender, EventArgs e)
        {
            // Connect to the server.
            string url = "rtmp://" + _textBoxServer.Text + ":" + System.Convert.ToInt32(_textBoxPort.Text) + "/" + _textBoxApplication.Text;
            _netConnection.Connect(url);
            //_netConnection.Connect(url, "user", "password");
        }

        private void _buttonRemoteCall_Click(object sender, EventArgs e)
        {
            _netConnection.Call("serverHelloMsg", this, "some text");
        }

        private void _buttonDisconnect_Click(object sender, EventArgs e)
        {
            _netConnection.Close();
            LogText("Disconnected.\r\n");
            UpdateState(false);
        }

        #region IPendingServiceCallback Members

        public void ResultReceived(IPendingServiceCall call)
        {
            object result = call.Result;
            LogText(string.Format("Server response: {0}\r\n", result));
        }

        #endregion

        //****************Make Thread-Safe Calls to Windows Forms Controls**********************

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);

        public void LogText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (_textBoxStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(LogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                _textBoxStatus.Text += text;
            }
        }

        public void LogASO(ASObject aso)
        {
            foreach (KeyValuePair<string, object> entry in aso)
            {
                LogText(string.Format("{0} : {1} \r\n", entry.Key, entry.Value));
            }
        }

        delegate void UpdateStateCallback(bool connected);

        public void UpdateState(bool connected)
        {
            if (_buttonConnect.InvokeRequired)
            {
                UpdateStateCallback d = new UpdateStateCallback(UpdateState);
                this.Invoke(d, new object[] { connected });
            }
            else
            {
                _buttonConnect.Enabled = !connected;
                _buttonRemoteCall.Enabled = connected;
                _buttonDisconnect.Enabled = connected;
            }
        }
    }
}