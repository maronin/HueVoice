using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Runtime.InteropServices;

namespace HueVoice
{
    
    public partial class HueShutdownForm : Form
    {
        HueForm parentForm;
        SpeechSynthesizer sSynth;
        
        public HueShutdownForm(HueForm parent, SpeechSynthesizer s)
        {
            parentForm = parent;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosed += HueShutdownForm_FormClosed;
            shutDownTimer.Start();
            sSynth = s;
            sSynth.SelectVoice("VW Julie");
            sSynth.SpeakAsync("initiating shutdown sequence!");

        }

        void HueShutdownForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            shutDownTimer.Stop();
            
        }

        private void shutDownTimer_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lblShutdownTimer.Text) > 0)
            {
                lblShutdownTimer.Text = (Convert.ToInt32(lblShutdownTimer.Text) - 1).ToString();
                
                
            }
            else
            {
                sSynth.Speak("Good night!");
                shutDownTimer.Stop();

                Process.Start("shutdown", "/s /t 2");
                parentForm.Close();
                
                
            }
        }

        private void btnCancelShutdown_Click(object sender, EventArgs e)
        {
                       
            this.Close();
        }



    }
}
