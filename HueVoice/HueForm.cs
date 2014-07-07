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

        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine recognizeCommands = new SpeechRecognitionEngine();
        HueBridge hueBridge;


        public HueForm()
        {

            hueBridge = new HueBridge("192.168.1.53", "homepagewebsite", this);
            InitializeComponent();



            List<HueLight> lights = hueBridge.getAllLights();
            trackBarSaturation.MouseUp += trackBarSaturation_MouseUp;



            initializeRecognizeCommands();

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



        public void initializeRecognizeCommands()
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            recognizeCommands.SpeechRecognized += recognizeCommands_SpeechRecognized;
            recognizeCommands.SetInputToDefaultAudioDevice();
            recognizeCommands.SpeechDetected += recognizeCommands_SpeechDetected;
            recognizeCommands.AudioLevelUpdated += AudioLevelUpdated;

            Choices sList = new Choices();

            for (int i = 1; i <= 100; i++)
            {
                sList.Add("roomy lights " + i + "%");
            }


            sList.Add(new string[] 
                { 
                    "roomy lights off", 
                    "roomy lights on", 
                    "roomy red lights", 
                    "roomy green lights",                   
                    "roomy blue lights",
                    "roomy white lights", 
                    "roomy orange lights",
                    "roomy yellow lights",
                    "roomy purple lights"
                });
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {

                recognizeCommands.RequestRecognizerUpdate();
                recognizeCommands.LoadGrammar(gr);
                recognizeCommands.LoadGrammar(defaultDictationGrammar);

            }

            catch
            {
                return;
            }
        }

        void recognizeCommands_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            micListeningBox.BackColor = Color.Red;
        }

        private void btnHueListen_Click(object sender, EventArgs e)
        {
            btnHueListen.Enabled = false;
            button3.Enabled = true;
            recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);

        }

        public void setConsoleText(string text)
        {
            if (text.Contains("success"))
            {
                tbConsoleOutput.AppendText(text + System.Environment.NewLine);
                HighlightPhrase(tbConsoleOutput, text, Color.Green);
            }
            else
            {
                tbConsoleOutput.AppendText(text + System.Environment.NewLine);

                HighlightPhrase(tbConsoleOutput, text, Color.Red);
            }



        }

        void AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {

            // verticalVolumeBar.Value = e.AudioLevel;

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
                        case "roomy lights off":
                            hueBridge.turnLightsOff("0");
                            tbConsoleOutput.AppendText("lights are off" + System.Environment.NewLine);
                            sSynth.SpeakAsync("turning lights, off");
                            break;
                        case "roomy lights on":
                            hueBridge.turnLightsOn("0");
                            tbConsoleOutput.AppendText("lights are on" + System.Environment.NewLine);
                            sSynth.SpeakAsync("turning lights, on");
                            break;
                        case "roomy red lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 0, 0), "0");
                            break;
                        case "roomy green lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 255, 0), "0");
                            break;
                        case "roomy blue lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 0, 255), "0");
                            break;
                        case "roomy yellow lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 0), "0");
                            break;
                        case "roomy orange lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 128, 0), "0");
                            break;
                        case "roomy white lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 255), "0");
                            break;
                        case "roomy purple lights":
                            hueBridge.changeLightColor(Color.FromArgb(127, 0, 255), "0");
                            break;
                        default:
                            //tbConsoleOutput.AppendText(e.Result.Text.ToString());
                            break;
                    }
                    if (command.Contains("%") && command.Contains("roomy"))
                    {
                        string[] dimPercentage = command.Split(' ');
                        hueBridge.setBrightness(dimPercentage[2], "0");
                    }
                }

            }

            tbVoiceOutput.AppendText(e.Result.Text.ToString() + System.Environment.NewLine);
            textBox3.Text = "";
            for (int i = 0; i < e.Result.Alternates.Count; i++)
            {

                textBox3.AppendText(e.Result.Alternates[i].Text + System.Environment.NewLine);
            }
            micListeningBox.BackColor = Color.DimGray;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            recognizeCommands.RecognizeAsyncStop();
            btnHueListen.Enabled = true;
            button3.Enabled = false;
        }

        private void btnToggleLights_Click(object sender, EventArgs e)
        {
            hueBridge.toggleLights("0");


        }

        static void HighlightPhrase(RichTextBox box, string phrase, Color color)
        {
            int pos = box.SelectionStart;
            string s = box.Text;
            for (int ix = 0; ; )
            {
                int jx = s.IndexOf(phrase, ix, StringComparison.CurrentCultureIgnoreCase);
                if (jx < 0) break;
                box.SelectionStart = jx;
                box.SelectionLength = phrase.Length;
                box.SelectionColor = color;
                ix = jx + 1;
            }
            box.SelectionStart = pos;
            box.SelectionLength = 0;
        }

        private void btnColorPicker_Click(object sender, EventArgs e)
        {
            hueColorDialog.ShowDialog();
            hueBridge.changeLightColor(hueColorDialog.Color, "0");
        }

        void trackBarSaturation_MouseUp(object sender, MouseEventArgs e)
        {
            hueBridge.changeSaturation(trackBarSaturation.Value, "0");

        }


    }
}
