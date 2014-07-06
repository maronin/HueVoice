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
using System.Speech.AudioFormat;
using System.Threading;
using System.Net;
using Newtonsoft.Json;

namespace HueVoice
{
    public partial class HueForm : Form
    {
        private WebClient client;
        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine recognizeHotWord = new SpeechRecognitionEngine();
        SpeechRecognitionEngine recognizeCommands = new SpeechRecognitionEngine();
        HueBridge test = new HueBridge("192.168.1.53", "homepagewebsite");
        public HueForm()
        {


            InitializeComponent();
            client = new WebClient();
            
            

            List<HueLight> lights = test.getAllLights();
            
            client.Proxy = null;

            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UploadStringCallback2);
            recognizeCommands.SpeechRecognized += recognizeCommands_SpeechRecognized;

            recognizeHotWord.SpeechDetected += recognizeHotWord_SpeechDetected;
            recognizeHotWord.AudioLevelUpdated += AudioLevelUpdated;
            recognizeHotWord.SpeechRecognized += recognizeHotWord_SpeechRecognized;
            recognizeHotWord.SetInputToDefaultAudioDevice();
            
            recognizeCommands.SetInputToDefaultAudioDevice();
            recognizeCommands.SpeechDetected += recognizeHotWord_SpeechDetected;
            recognizeCommands.AudioLevelUpdated += AudioLevelUpdated;

            sSynth.SelectVoice("VW Julie");
            sSynth.Volume = 100;
            sSynth.Rate = 0;
            //sSynth.SelectVoice(

            /*
            SpeechSynthesizer synth = new SpeechSynthesizer();
            

                // Output information about all of the installed voices. 
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            */



        }

        void recognizeHotWord_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            textBox2.BackColor = Color.Red;
        }


        public void hueListen()
        {
            if (!client.IsBusy)
            {
                var GetUri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0", "192.168.1.53"));


                var jsonThing = client.DownloadString(GetUri); //how to GET from a URL
                dynamic data = JsonConvert.DeserializeObject(jsonThing);
                Console.WriteLine(data["action"].on);

                var putUri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));
                var reg = new
                {
                    hue = 25718
                };

                var jsonObj = JsonConvert.SerializeObject(reg);

                if (!client.IsBusy)
                {
                    client.UploadStringAsync(putUri, "PUT", jsonObj);
                    textBox1.AppendText("Listening..." + System.Environment.NewLine);
                    sSynth.SpeakAsync("listening...");


                }
            }

        }

        public void turnOffLights()
        {

            if (!client.IsBusy)
            {
                var uri = new Uri(string.Format("http://{0}/api/homepagewebsite/groups/0/action", "192.168.1.53"));

                var reg = new
                {
                    on = false
                };

                var jsonObj = JsonConvert.SerializeObject(reg);

                client.UploadStringAsync(uri, "PUT", jsonObj);
                textBox1.AppendText("lights are off" + System.Environment.NewLine);
                sSynth.SpeakAsync("turning lights off");

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
                    on = true,
                    bri = 254
                };

                var jsonObj = JsonConvert.SerializeObject(reg);

                client.UploadStringAsync(uri, "PUT", jsonObj);
                textBox1.AppendText("lights are on" + System.Environment.NewLine);
                sSynth.SpeakAsync("turning lights on");
            }

        }




        void UploadStringCallback2(object sender, UploadStringCompletedEventArgs e)
        {
            textBox1.AppendText(e.Result + System.Environment.NewLine);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(textBox1.Text);
            sSynth.Speak(pBuilder);
        }

        private void btnHueListen_Click(object sender, EventArgs e)
        {


            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;



            btnHueListen.Enabled = false;
            button3.Enabled = true;
            Choices sList = new Choices();
            sList.Add(new string[] { "ok room", "roomy", "roomy lights off", "roomy lights on" });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {

                recognizeCommands.RequestRecognizerUpdate();
                recognizeCommands.LoadGrammar(gr);
                recognizeCommands.LoadGrammar(defaultDictationGrammar);

                recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);
                recognizeCommands.Recognize();

            }

            catch
            {
                return;
            }




        }

        private void recognizeHotWord_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            string command = e.Result.Text.ToString().ToLower();
            if (e.Result.Confidence > 0.6)
            {

                //textBox1.Text = e.Result.Text.ToString();

                if (e.Result.Text == "exit")
                {
                    Application.Exit();
                }
                //else if (command == "ok room" || command == "okay room")
                // {

                // hueListen();


                Choices sList = new Choices();
                sList.Add(new string[] { "roomy lights off", "roomy lights on", "what's the current weather" });
                Grammar gr = new Grammar(new GrammarBuilder(sList));
                try
                {

                    recognizeCommands.RequestRecognizerUpdate();
                    recognizeCommands.LoadGrammar(gr);
                    recognizeCommands.LoadGrammar(defaultDictationGrammar);
                    recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);
                    recognizeCommands.Recognize();
                    recognizeCommands.AudioLevelUpdated += AudioLevelUpdated;

                }

                catch
                {
                    return;
                }

            }

            //}

            else
            {

            }
            tbVoiceOutput.AppendText(e.Result.Text.ToString() + System.Environment.NewLine);
            textBox3.Text = "";
            for (int i = 0; i < e.Result.Alternates.Count; i++)
            {

                textBox3.AppendText(e.Result.Alternates[i].Text + System.Environment.NewLine);
            }
            textBox2.BackColor = Color.White;
        }

        void AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {

            verticalVolumeBar.Value = e.AudioLevel;

        }

        private void recognizeCommands_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "exit")
            {
                Application.Exit();
            }
            else
            {
                if (e.Result.Confidence > 0.7)
                {
                    if (e.Result.Text.ToString() == "roomy lights off")
                    {
                        turnOffLights();
                    }

                    else if (e.Result.Text.ToString() == "roomy lights on")
                    {
                        turnOnLights();
                    }
                    else
                    {
                        textBox1.AppendText(e.Result.Text.ToString());
                    }
                    //Stop listening for commands
                    //recognizeCommands.RecognizeAsyncStop();
                }

            }
            textBox2.BackColor = Color.White;
            tbVoiceOutput.AppendText(e.Result.Text.ToString() + System.Environment.NewLine);
            textBox3.Text = "";
            for (int i = 0; i < e.Result.Alternates.Count; i++)
            {

                textBox3.AppendText(e.Result.Alternates[i].Text + System.Environment.NewLine);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            recognizeHotWord.RecognizeAsyncStop();
            btnHueListen.Enabled = true;
            button3.Enabled = false;
        }

        private void btnToggleLights_Click(object sender, EventArgs e)
        {
            test.toggleLights("0");
        }





    }
}
