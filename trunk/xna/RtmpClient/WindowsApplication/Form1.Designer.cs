namespace WindowsApplication
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._buttonRemoteCall = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._textBoxServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._textBoxPort = new System.Windows.Forms.TextBox();
            this._textBoxApplication = new System.Windows.Forms.TextBox();
            this._textBoxStatus = new System.Windows.Forms.TextBox();
            this._buttonConnect = new System.Windows.Forms.Button();
            this._buttonDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _buttonRemoteCall
            // 
            this._buttonRemoteCall.Location = new System.Drawing.Point(15, 64);
            this._buttonRemoteCall.Name = "_buttonRemoteCall";
            this._buttonRemoteCall.Size = new System.Drawing.Size(117, 23);
            this._buttonRemoteCall.TabIndex = 0;
            this._buttonRemoteCall.Text = "serverHelloMsg";
            this._buttonRemoteCall.UseVisualStyleBackColor = true;
            this._buttonRemoteCall.Click += new System.EventHandler(this._buttonRemoteCall_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // _textBoxServer
            // 
            this._textBoxServer.Location = new System.Drawing.Point(56, 6);
            this._textBoxServer.Name = "_textBoxServer";
            this._textBoxServer.Size = new System.Drawing.Size(119, 20);
            this._textBoxServer.TabIndex = 2;
            this._textBoxServer.Text = "localhost";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(298, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "RTM Application";
            // 
            // _textBoxPort
            // 
            this._textBoxPort.Location = new System.Drawing.Point(230, 6);
            this._textBoxPort.Name = "_textBoxPort";
            this._textBoxPort.Size = new System.Drawing.Size(40, 20);
            this._textBoxPort.TabIndex = 2;
            this._textBoxPort.Text = "1935";
            // 
            // _textBoxApplication
            // 
            this._textBoxApplication.Location = new System.Drawing.Point(400, 6);
            this._textBoxApplication.Name = "_textBoxApplication";
            this._textBoxApplication.Size = new System.Drawing.Size(145, 20);
            this._textBoxApplication.TabIndex = 2;
            this._textBoxApplication.Text = "HelloWorld";
            // 
            // _textBoxStatus
            // 
            this._textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxStatus.Location = new System.Drawing.Point(15, 122);
            this._textBoxStatus.Multiline = true;
            this._textBoxStatus.Name = "_textBoxStatus";
            this._textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxStatus.Size = new System.Drawing.Size(530, 233);
            this._textBoxStatus.TabIndex = 3;
            // 
            // _buttonConnect
            // 
            this._buttonConnect.Location = new System.Drawing.Point(15, 35);
            this._buttonConnect.Name = "_buttonConnect";
            this._buttonConnect.Size = new System.Drawing.Size(117, 23);
            this._buttonConnect.TabIndex = 4;
            this._buttonConnect.Text = "Connect";
            this._buttonConnect.UseVisualStyleBackColor = true;
            this._buttonConnect.Click += new System.EventHandler(this._buttonConnect_Click);
            // 
            // _buttonDisconnect
            // 
            this._buttonDisconnect.Location = new System.Drawing.Point(15, 93);
            this._buttonDisconnect.Name = "_buttonDisconnect";
            this._buttonDisconnect.Size = new System.Drawing.Size(117, 23);
            this._buttonDisconnect.TabIndex = 4;
            this._buttonDisconnect.Text = "Disconnect";
            this._buttonDisconnect.UseVisualStyleBackColor = true;
            this._buttonDisconnect.Click += new System.EventHandler(this._buttonDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 367);
            this.Controls.Add(this._buttonDisconnect);
            this.Controls.Add(this._buttonConnect);
            this.Controls.Add(this._textBoxStatus);
            this.Controls.Add(this._textBoxApplication);
            this.Controls.Add(this._textBoxPort);
            this.Controls.Add(this._textBoxServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._buttonRemoteCall);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _buttonRemoteCall;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _textBoxServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _textBoxPort;
        private System.Windows.Forms.TextBox _textBoxApplication;
        private System.Windows.Forms.TextBox _textBoxStatus;
        private System.Windows.Forms.Button _buttonConnect;
        private System.Windows.Forms.Button _buttonDisconnect;
    }
}

