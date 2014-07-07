﻿namespace HueVoice
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
            this.btnHueListen = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnToggleLights = new System.Windows.Forms.Button();
            this.tbVoiceOutput = new System.Windows.Forms.TextBox();
            this.micListeningBox = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hueColorDialog = new System.Windows.Forms.ColorDialog();
            this.btnColorPicker = new System.Windows.Forms.Button();
            this.trackBarSaturation = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSaturation)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHueListen
            // 
            this.btnHueListen.Location = new System.Drawing.Point(87, 18);
            this.btnHueListen.Name = "btnHueListen";
            this.btnHueListen.Size = new System.Drawing.Size(75, 23);
            this.btnHueListen.TabIndex = 2;
            this.btnHueListen.Text = "listen";
            this.btnHueListen.UseVisualStyleBackColor = true;
            this.btnHueListen.Click += new System.EventHandler(this.btnHueListen_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(87, 48);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnToggleLights
            // 
            this.btnToggleLights.Location = new System.Drawing.Point(192, 19);
            this.btnToggleLights.Name = "btnToggleLights";
            this.btnToggleLights.Size = new System.Drawing.Size(75, 52);
            this.btnToggleLights.TabIndex = 6;
            this.btnToggleLights.Text = "Toggle Lights";
            this.btnToggleLights.UseVisualStyleBackColor = true;
            this.btnToggleLights.Click += new System.EventHandler(this.btnToggleLights_Click);
            // 
            // tbVoiceOutput
            // 
            this.tbVoiceOutput.Location = new System.Drawing.Point(414, 308);
            this.tbVoiceOutput.Multiline = true;
            this.tbVoiceOutput.Name = "tbVoiceOutput";
            this.tbVoiceOutput.Size = new System.Drawing.Size(273, 70);
            this.tbVoiceOutput.TabIndex = 7;
            // 
            // micListeningBox
            // 
            this.micListeningBox.BackColor = System.Drawing.Color.DimGray;
            this.micListeningBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.micListeningBox.Location = new System.Drawing.Point(25, 41);
            this.micListeningBox.Multiline = true;
            this.micListeningBox.Name = "micListeningBox";
            this.micListeningBox.Size = new System.Drawing.Size(31, 30);
            this.micListeningBox.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(738, 12);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(101, 459);
            this.textBox3.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHueListen);
            this.groupBox1.Controls.Add(this.micListeningBox);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.btnToggleLights);
            this.groupBox1.Location = new System.Drawing.Point(389, 384);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 90);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tbConsoleOutput
            // 
            this.tbConsoleOutput.BackColor = System.Drawing.Color.Black;
            this.tbConsoleOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbConsoleOutput.ForeColor = System.Drawing.Color.White;
            this.tbConsoleOutput.Location = new System.Drawing.Point(12, 12);
            this.tbConsoleOutput.Name = "tbConsoleOutput";
            this.tbConsoleOutput.ReadOnly = true;
            this.tbConsoleOutput.Size = new System.Drawing.Size(675, 290);
            this.tbConsoleOutput.TabIndex = 11;
            this.tbConsoleOutput.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.trackBarSaturation);
            this.groupBox2.Controls.Add(this.btnColorPicker);
            this.groupBox2.Location = new System.Drawing.Point(12, 308);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 166);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Light Color";
            // 
            // btnColorPicker
            // 
            this.btnColorPicker.Location = new System.Drawing.Point(6, 19);
            this.btnColorPicker.Name = "btnColorPicker";
            this.btnColorPicker.Size = new System.Drawing.Size(75, 23);
            this.btnColorPicker.TabIndex = 14;
            this.btnColorPicker.Text = "Pick Color";
            this.btnColorPicker.UseVisualStyleBackColor = true;
            this.btnColorPicker.Click += new System.EventHandler(this.btnColorPicker_Click);
            // 
            // trackBarSaturation
            // 
            this.trackBarSaturation.Location = new System.Drawing.Point(6, 76);
            this.trackBarSaturation.Maximum = 255;
            this.trackBarSaturation.Name = "trackBarSaturation";
            this.trackBarSaturation.Size = new System.Drawing.Size(188, 45);
            this.trackBarSaturation.TabIndex = 14;
            this.trackBarSaturation.TickFrequency = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Saturation";
            // 
            // HueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 483);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbConsoleOutput);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.tbVoiceOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HueForm";
            this.Text = "Hue Voice";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSaturation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHueListen;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox micListeningBox;
        private System.Windows.Forms.Button btnToggleLights;
        private System.Windows.Forms.TextBox tbVoiceOutput;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox tbConsoleOutput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColorDialog hueColorDialog;
        private System.Windows.Forms.Button btnColorPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarSaturation;
        
    }
}

