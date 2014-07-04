using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Threading;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System.Net;
using Newtonsoft.Json;

namespace HueVoice
{
    public partial class Form1 : Form
    {
        private WebClient client;
        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        SpeechRecognitionEngine sRecognize2 = new SpeechRecognitionEngine();

        public Form1()
        {
            
            InitializeComponent();
            client = new WebClient();
            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UploadStringCallback2);
            sRecognize2.SpeechRecognized += sRecognize_SpeechRecognized2;
            sRecognize.SpeechRecognized += sRecognize_SpeechRecognized;
            sRecognize.SetInputToDefaultAudioDevice();
            sRecognize2.SetInputToDefaultAudioDevice();


            
        }


        public void hueListen()
        {
            if (!client.IsBusy)
            {
                var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

                var reg = new
                {
                    hue = 50000
                };

                var jsonObj = JsonConvert.SerializeObject(reg);

                client.UploadStringAsync(uri, "PUT", jsonObj);
                textBox1.AppendText(System.Environment.NewLine + "Listening...");
            }
            
        }

        public void turnOffLights()
        {

          if (!client.IsBusy)
          {
              var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

              var reg = new
              {
                  on =  false
              };

              var jsonObj = JsonConvert.SerializeObject(reg);

              client.UploadStringAsync(uri, "PUT", jsonObj);
              textBox1.AppendText(System.Environment.NewLine + "lights are off");
              
          }

         
        }

        public void turnOnLights()
        {

            if (!client.IsBusy)
            {
                client = new WebClient();
                var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

                var reg = new
                {
                    on = true
                };

                var jsonObj = JsonConvert.SerializeObject(reg);

                client.UploadStringAsync(uri, "PUT", jsonObj);
                textBox1.AppendText(System.Environment.NewLine + "lights are on");
            }

        }




        void UploadStringCallback2(object sender, UploadStringCompletedEventArgs e)
        {
            //textBox1.AppendText(e.Result);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(textBox1.Text);
            sSynth.Speak(pBuilder);
        }

        private void button2_Click(object sender, EventArgs e)
        {


            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;



            button2.Enabled = false;
            button3.Enabled = true;
            Choices sList = new Choices();
            sList.Add(new string[] { "ok room" });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {
               
                sRecognize.RequestRecognizerUpdate();
                sRecognize.LoadGrammar(gr);
                sRecognize.LoadGrammar(defaultDictationGrammar);
                sRecognize.SpeechRecognized += sRecognize_SpeechRecognized;
                
                textBox1.Text = sRecognize.AudioLevel.ToString();
                sRecognize.AudioLevelUpdated += sRecognize2_AudioLevelUpdated;
                sRecognize.RecognizeAsync(RecognizeMode.Multiple);
                sRecognize.Recognize();

            }

            catch
            {
                return;
            }




        }

        private void sRecognize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;


            if (e.Result.Confidence > 0.6)
            {

                textBox1.Text = e.Result.Text.ToString();
                string command = e.Result.Text.ToString().ToLower();
                if (e.Result.Text == "exit")
                {
                    Application.Exit();
                }
                else if (command == "ok room" || command == "okay room")
                {

                    hueListen();
                    Choices sList = new Choices();
                    sList.Add(new string[] { "lights off", "lights on", "what's the current weather" });
                    Grammar gr = new Grammar(new GrammarBuilder(sList));
                    try
                    {

                        sRecognize2.RequestRecognizerUpdate();
                        sRecognize2.LoadGrammar(gr);
                        sRecognize2.LoadGrammar(defaultDictationGrammar);
                        sRecognize2.RecognizeAsync(RecognizeMode.Multiple);
                        sRecognize2.Recognize();
                        sRecognize2.AudioLevelUpdated += sRecognize2_AudioLevelUpdated;

                    }

                    catch
                    {
                        return;
                    }

                }
            }
            else
            {
                textBox1.Text = "what? " + e.Result.Text.ToString();
            }

        }

        void sRecognize2_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            
            verticalVolumeBar.Value = e.AudioLevel;
           
        }

        private void sRecognize_SpeechRecognized2(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "exit")
            {
                Application.Exit();
            }
            else
            {
                if (e.Result.Confidence > 0.6)
                {
                    if (e.Result.Text.ToString() == "lights off")
                    {
                        turnOffLights();
                    }

                    else if (e.Result.Text.ToString() == "lights on")
                    {
                        turnOnLights();
                    }
                    else
                    {
                        textBox1.AppendText(e.Result.Text.ToString());
                    }
                    //Stop listening for commands
                    sRecognize2.RecognizeAsyncStop();
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            sRecognize.RecognizeAsyncStop();
            button2.Enabled = true;
            button3.Enabled = false;
        }

        

    }
}
