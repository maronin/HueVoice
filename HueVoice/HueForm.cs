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
        HueBridge hueBridge = new HueBridge("192.168.1.53", "homepagewebsite");
        public HueForm()
        {


            InitializeComponent();
            client = new WebClient();



            List<HueLight> lights = hueBridge.getAllLights();

            client.Proxy = null;

            client.UploadStringCompleted += new UploadStringCompletedEventHandler(UploadStringCallback2);


            initializeRecognizeCommands();
            initializeRecognizeHotWord();         

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


        public void initializeRecognizeHotWord()
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            recognizeHotWord.SpeechRecognized += recognizeHotWord_SpeechRecognized;
            recognizeHotWord.SetInputToDefaultAudioDevice();
            recognizeHotWord.SpeechDetected += recognizeHotWord_SpeechDetected;
            recognizeHotWord.AudioLevelUpdated += AudioLevelUpdated;
            

            Choices sList = new Choices();
            sList.Add(new string[] 
                { 
                    "roomy"
                });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {

                recognizeHotWord.RequestRecognizerUpdate();
                recognizeHotWord.LoadGrammar(gr);
                //recognizeHotWord.LoadGrammar(defaultDictationGrammar);
                

            }

            catch
            {
                return;
            }
        }

        public void initializeRecognizeCommands()
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            recognizeCommands.SpeechRecognized += recognizeCommands_SpeechRecognized;
            recognizeCommands.SetInputToDefaultAudioDevice();
            recognizeCommands.SpeechDetected += recognizeHotWord_SpeechDetected;
            recognizeCommands.AudioLevelUpdated += AudioLevelUpdated;

            Choices sList = new Choices();

            for (int i = 1; i <= 100; i++)
            {
                sList.Add("lights " + i + "%");
            }


            sList.Add(new string[] 
                { 
                    "lights off", 
                    "lights on", 
                    "red lights", 
                    "green lights",                   
                    "blue lights",
                    "white lights", 
                    "orange lights",
                    "yellow lights",
                    "purple lights"
                });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {

                recognizeCommands.RequestRecognizerUpdate();
                recognizeCommands.LoadGrammar(gr);
               // recognizeCommands.LoadGrammar(defaultDictationGrammar);

                //recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);
                //recognizeCommands.Recognize();
                
            }

            catch
            {
                return;
            }
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

        void UploadStringCallback2(object sender, UploadStringCompletedEventArgs e)
        {
            //textBox1.AppendText(e.Result + System.Environment.NewLine);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(textBox1.Text);
            sSynth.Speak(pBuilder);
        }

        private void btnHueListen_Click(object sender, EventArgs e)
        {
            btnHueListen.Enabled = false;
            button3.Enabled = true;
            
            recognizeHotWord.RecognizeAsync(RecognizeMode.Multiple);
            

        }

        private void recognizeHotWord_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            string command = e.Result.Text.ToString().ToLower();
            if (e.Result.Confidence > 0.6)
            {


                if (e.Result.Text == "exit")
                {
                    Application.Exit();
                }

                else if (command == "roomy")
                {
                    try
                    {
                        recognizeHotWord.RecognizeAsyncStop();
                        recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);
                        
                        //recognizeCommands.LoadGrammar(defaultDictationGrammar);




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

                string command = e.Result.Text.ToString().ToLower();

                if (e.Result.Confidence > 0.7)
                {
                    switch (command)
                    {
                        case "lights off":
                            hueBridge.turnLightsOff("0");
                            textBox1.AppendText("lights are off" + System.Environment.NewLine);
                             sSynth.SpeakAsync("turning lights, off");
                            break;
                        case "lights on":
                            hueBridge.turnLightsOn("0");
                            textBox1.AppendText("lights are on" + System.Environment.NewLine);
                             sSynth.SpeakAsync("turning lights, on");
                            break;
                        case "red lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 0, 0), "0");
                            break;
                        case "green lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 255, 0), "0");
                            break;
                        case "blue lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 0, 255), "0");
                            break;
                        case "yellow lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 0), "0");
                            break;
                        case "orange lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 128, 0), "0");
                            break;
                        case "white lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 255), "0");
                            break;
                        case "purple lights":
                            hueBridge.changeLightColor(Color.FromArgb(127, 0, 255), "0");
                            break;
                        default:
                            textBox1.AppendText(e.Result.Text.ToString());
                            break;
                    }
                    if (command.Contains("%"))
                    {
                        string[] dimPercentage = command.Split(' ');
                        hueBridge.dimLights(dimPercentage[2], "0");
                    }
                }
                recognizeCommands.RecognizeAsyncStop();
                recognizeHotWord.RecognizeAsync(RecognizeMode.Multiple);

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
            hueBridge.toggleLights("0");


        }



    }
}
