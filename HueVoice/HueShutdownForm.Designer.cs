namespace HueVoice
{
    partial class HueShutdownForm
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
            this.lblShutdownText = new System.Windows.Forms.Label();
            this.btnCancelShutdown = new System.Windows.Forms.Button();
            this.lblShutdownTimer = new System.Windows.Forms.Label();
            this.shutDownTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblShutdownText
            // 
            this.lblShutdownText.AutoSize = true;
            this.lblShutdownText.Location = new System.Drawing.Point(35, 9);
            this.lblShutdownText.Name = "lblShutdownText";
            this.lblShutdownText.Size = new System.Drawing.Size(135, 13);
            this.lblShutdownText.TabIndex = 0;
            this.lblShutdownText.Text = "Computer will shutdown in: ";
            // 
            // btnCancelShutdown
            // 
            this.btnCancelShutdown.Location = new System.Drawing.Point(70, 42);
            this.btnCancelShutdown.Name = "btnCancelShutdown";
            this.btnCancelShutdown.Size = new System.Drawing.Size(133, 23);
            this.btnCancelShutdown.TabIndex = 1;
            this.btnCancelShutdown.Text = "Cancel Shutdown";
            this.btnCancelShutdown.UseVisualStyleBackColor = true;
            this.btnCancelShutdown.Click += new System.EventHandler(this.btnCancelShutdown_Click);
            // 
            // lblShutdownTimer
            // 
            this.lblShutdownTimer.AutoSize = true;
            this.lblShutdownTimer.Location = new System.Drawing.Point(189, 9);
            this.lblShutdownTimer.Name = "lblShutdownTimer";
            this.lblShutdownTimer.Size = new System.Drawing.Size(19, 13);
            this.lblShutdownTimer.TabIndex = 2;
            this.lblShutdownTimer.Text = "10";
            // 
            // shutDownTimer
            // 
            this.shutDownTimer.Interval = 1000;
            this.shutDownTimer.Tick += new System.EventHandler(this.shutDownTimer_Tick);
            // 
            // HueShutdownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 77);
            this.Controls.Add(this.lblShutdownTimer);
            this.Controls.Add(this.btnCancelShutdown);
            this.Controls.Add(this.lblShutdownText);
            this.Name = "HueShutdownForm";
            this.Text = "HueShutdownForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblShutdownText;
        private System.Windows.Forms.Button btnCancelShutdown;
        private System.Windows.Forms.Label lblShutdownTimer;
        private System.Windows.Forms.Timer shutDownTimer;
    }
}