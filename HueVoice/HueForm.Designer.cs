namespace HueVoice
{
    partial class HueForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HueForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnHueListen = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnToggleLights = new System.Windows.Forms.Button();
            this.tbVoiceOutput = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.verticalVolumeBar = new HueVoice.verticalBar();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(273, 155);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnHueListen
            // 
            this.btnHueListen.Location = new System.Drawing.Point(103, 275);
            this.btnHueListen.Name = "btnHueListen";
            this.btnHueListen.Size = new System.Drawing.Size(75, 23);
            this.btnHueListen.TabIndex = 2;
            this.btnHueListen.Text = "listen";
            this.btnHueListen.UseVisualStyleBackColor = true;
            this.btnHueListen.Click += new System.EventHandler(this.btnHueListen_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(184, 275);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnToggleLights
            // 
            this.btnToggleLights.Location = new System.Drawing.Point(331, 274);
            this.btnToggleLights.Name = "btnToggleLights";
            this.btnToggleLights.Size = new System.Drawing.Size(75, 52);
            this.btnToggleLights.TabIndex = 6;
            this.btnToggleLights.Text = "Toggle Lights";
            this.btnToggleLights.UseVisualStyleBackColor = true;
            this.btnToggleLights.Click += new System.EventHandler(this.btnToggleLights_Click);
            // 
            // tbVoiceOutput
            // 
            this.tbVoiceOutput.Location = new System.Drawing.Point(12, 173);
            this.tbVoiceOutput.Multiline = true;
            this.tbVoiceOutput.Name = "tbVoiceOutput";
            this.tbVoiceOutput.Size = new System.Drawing.Size(273, 70);
            this.tbVoiceOutput.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(379, 11);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(31, 235);
            this.textBox2.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(300, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(63, 231);
            this.textBox3.TabIndex = 9;
            // 
            // verticalVolumeBar
            // 
            this.verticalVolumeBar.ForeColor = System.Drawing.Color.Lime;
            this.verticalVolumeBar.Location = new System.Drawing.Point(380, 12);
            this.verticalVolumeBar.Name = "verticalVolumeBar";
            this.verticalVolumeBar.Size = new System.Drawing.Size(29, 233);
            this.verticalVolumeBar.TabIndex = 5;
            // 
            // HueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 339);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.verticalVolumeBar);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.tbVoiceOutput);
            this.Controls.Add(this.btnToggleLights);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnHueListen);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HueForm";
            this.Text = "Hue Voice";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnHueListen;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox2;
        private verticalBar verticalVolumeBar;
        private System.Windows.Forms.Button btnToggleLights;
        private System.Windows.Forms.TextBox tbVoiceOutput;
        private System.Windows.Forms.TextBox textBox3;
        
    }
}

