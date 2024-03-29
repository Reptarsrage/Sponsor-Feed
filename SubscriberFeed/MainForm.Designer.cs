﻿namespace SubscriberFeed
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.LogTextBox = new System.Windows.Forms.RichTextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.QueryTimer = new System.Windows.Forms.Timer(this.components);
            this.DeleteLocalButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LogTextBox
            // 
            this.LogTextBox.AcceptsTab = true;
            this.LogTextBox.Location = new System.Drawing.Point(13, 12);
            this.LogTextBox.MaxLength = 0;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.LogTextBox.Size = new System.Drawing.Size(370, 502);
            this.LogTextBox.TabIndex = 0;
            this.LogTextBox.Text = "";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(390, 13);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(390, 43);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // QueryTimer
            // 
            this.QueryTimer.Interval = 1000;
            this.QueryTimer.Tick += new System.EventHandler(this.QueryTimer_Tick);
            // 
            // DeleteLocalButton
            // 
            this.DeleteLocalButton.Location = new System.Drawing.Point(695, 13);
            this.DeleteLocalButton.Name = "DeleteLocalButton";
            this.DeleteLocalButton.Size = new System.Drawing.Size(107, 23);
            this.DeleteLocalButton.TabIndex = 3;
            this.DeleteLocalButton.Text = "Delete Local Data";
            this.DeleteLocalButton.UseVisualStyleBackColor = true;
            this.DeleteLocalButton.Click += new System.EventHandler(this.DeleteLocalButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(695, 43);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(106, 23);
            this.TestButton.TabIndex = 4;
            this.TestButton.Text = "Test Alert";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 526);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.DeleteLocalButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.LogTextBox);
            this.Name = "MainForm";
            this.Text = "Subscriber Feed";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox LogTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Timer QueryTimer;
        private System.Windows.Forms.Button DeleteLocalButton;
        private System.Windows.Forms.Button TestButton;
    }
}

