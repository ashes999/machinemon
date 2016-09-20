namespace MachineMon.Client
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
            this.uxSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.uxMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // uxSend
            // 
            this.uxSend.Location = new System.Drawing.Point(683, 45);
            this.uxSend.Name = "uxSend";
            this.uxSend.Size = new System.Drawing.Size(75, 23);
            this.uxSend.TabIndex = 0;
            this.uxSend.Text = "Send";
            this.uxSend.UseVisualStyleBackColor = true;
            this.uxSend.Click += new System.EventHandler(this.uxSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(745, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Make sure RabbitMQ is running locally. (The website should not display any except" +
    "ions.)  Type a message below and click the button to send it to the server.";
            // 
            // uxMessage
            // 
            this.uxMessage.Location = new System.Drawing.Point(16, 48);
            this.uxMessage.Name = "uxMessage";
            this.uxMessage.Size = new System.Drawing.Size(661, 20);
            this.uxMessage.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.uxMessage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uxSend);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button uxSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uxMessage;
    }
}

