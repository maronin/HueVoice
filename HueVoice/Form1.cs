using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private IHueClient _client;


        public Form1()
        {
            InitializeComponent();
            
        }


        public void turnOffLights()
        {
          string ip = "http://192.168.1.53";
          string key = "homepagewebsite";

          _client = new HueClient(ip, key);
          var client = new WebClient();
          var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

          var reg = new
          {
              on =  false
          };




          var jsonObj = JsonConvert.SerializeObject(reg);

          client.UploadStringAsync(uri, "PUT", jsonObj);
          client.UploadStringCompleted
               += new UploadStringCompletedEventHandler(UploadStringCallback2);

          

        }

        public void turnOnLights()
        {
            string ip = "http://192.168.1.53";
            string key = "homepagewebsite";

            _client = new HueClient(ip, key);
            var client = new WebClient();
            var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

            var reg = new
            {
                on = true
            };




            var jsonObj = JsonConvert.SerializeObject(reg);

            client.UploadStringAsync(uri, "PUT", jsonObj);
            client.UploadStringCompleted
                 += new UploadStringCompletedEventHandler(UploadStringCallback2);



        }


        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();

        void UploadStringCallback2(object sender, UploadStringCompletedEventArgs e)
        {
            textBox1.Text = e.Result;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(textBox1.Text);
            sSynth.Speak(pBuilder);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;
            Choices sList = new Choices();
            sList.Add(new string[] { "lights off", "lights on", "it works", "how", "are", "you", "today", "i", "am", "fine", "exit", "close", "quit", "so" });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {
                sRecognize.RequestRecognizerUpdate();
                sRecognize.LoadGrammar(gr);
                sRecognize.SpeechRecognized += sRecognize_SpeechRecognized;
                sRecognize.SetInputToDefaultAudioDevice();
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
            if (e.Result.Text == "exit")
            {
                Application.Exit();
            }
            else
            {
                
                if (e.Result.Text.ToString() == "lights off")
                {
                    turnOffLights();
                }

                if (e.Result.Text.ToString() == "lights on")
                {
                    turnOnLights();
                }
                
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            sRecognize.RecognizeAsyncStop();
            button2.Enabled = true;
            button3.Enabled = false;
        }

        public async Task SendCommandAsync()
        {
            //Create command
            var command = new LightCommand();
            command.TurnOn();
            command.SetColor("#225566");

            List<string> lights = new List<string>();

            //Send Command
            await _client.SendCommandAsync(command);
            await _client.SendCommandAsync(command, lights);

        }
    }
}
