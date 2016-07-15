namespace SubscriberFeed
{
    partial class EnterTokenForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDone = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelToken = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.linkLabelAuth = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(13, 118);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(389, 117);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(124, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(340, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelToken
            // 
            this.labelToken.AutoSize = true;
            this.labelToken.Location = new System.Drawing.Point(12, 83);
            this.labelToken.Name = "labelToken";
            this.labelToken.Size = new System.Drawing.Size(106, 13);
            this.labelToken.TabIndex = 3;
            this.labelToken.Text = "Paste the token here";
            // 
            // textBox2
            // 
            this.textBox2.AcceptsReturn = true;
            this.textBox2.AcceptsTab = true;
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(15, 13);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(449, 38);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "Please click on the link below and verify that this program can access your accou" +
    "nt. You will then be presented with a token, please copy and paste the token bel" +
    "ow.";
            // 
            // linkLabelAuth
            // 
            this.linkLabelAuth.AutoSize = true;
            this.linkLabelAuth.Location = new System.Drawing.Point(15, 49);
            this.linkLabelAuth.Name = "linkLabelAuth";
            this.linkLabelAuth.Size = new System.Drawing.Size(51, 13);
            this.linkLabelAuth.TabIndex = 5;
            this.linkLabelAuth.TabStop = true;
            this.linkLabelAuth.Text = "Authorize";
            this.linkLabelAuth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAuth_LinkClicked);
            // 
            // EnterTokenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 153);
            this.Controls.Add(this.linkLabelAuth);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.labelToken);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.buttonCancel);
            this.Name = "EnterTokenForm";
            this.Text = "Enter Token";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDone;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelToken;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.LinkLabel linkLabelAuth;
    }
}