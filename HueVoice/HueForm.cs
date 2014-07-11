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
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.IO;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
namespace HueVoice
{
    public partial class HueForm : Form
    {

        SpeechSynthesizer sSynth = new SpeechSynthesizer();
        PromptBuilder pBuilder = new PromptBuilder();
        SpeechRecognitionEngine recognizeCommands = new SpeechRecognitionEngine();
        HueBridge hueBridge;
        HueShutdownForm shutdownForm;
        WaveOut player;
        AudioFileReader wavFile;
        public HueForm()
        {
            InitializeComponent();
            //send over the ip, user id and the current form reference
            hueBridge = new HueBridge("192.168.1.53", "homepagewebsite", this);

            drawSynthVolumeUI();

            this.FormClosed += HueForm_FormClosed;



            //Saturation slider
            trackBarSaturation.MouseUp += trackBarSaturation_MouseUp;

            initializeRecognizeCommands();

            //which voice is used
            sSynth.SelectVoice("VW Julie");
            //how fast the voice talks
            sSynth.Rate = 0;

            





        }

        void player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            for (int i = 1; i <= 25; i++)
            {
                RoundButton roundButton = (RoundButton)Controls["synthVolume" + i];
                roundButton.BackColor = Color.FromArgb(0, 0, 0);
            }
            wavFile.Close();
        }

        void onPostVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            double audioLevel = e.MaxSampleValues[0];

            audioLevel *= 400;

            for (int i = 1; i <= 25; i++)
            {
                
                RoundButton roundButton = (RoundButton)Controls["synthVolume" + i];
               // audioLevel -= i;
                audioLevel = (audioLevel - i);

                if (audioLevel > 255)
                {
                    audioLevel = 255;
                }
                else if (audioLevel < 0)
                {
                    audioLevel = 0;
                }
                
                roundButton.BackColor = Color.FromArgb((int)audioLevel, 0, 0);

            }

        }


        //the application has been closed
        void HueForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            recognizeCommands.RecognizeAsyncStop();

            Environment.Exit(-1);

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

            //make light percentage grammer
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
                    "roomy purple lights",
                    "computer shutdown",
                    "computer open excel",
                    "computer open word",
                    "computer start TF2",
                    "computer check mail",
                    "computer open you tube"

                });
            Grammar gr = new Grammar(new GrammarBuilder(sList));

            //Make grammer for unit conversion
            Choices conversionCommands = new Choices();
            conversionCommands.Add("computer convert ");


            //SemanticResultKey srkComtype = new SemanticResultKey("conversion command", conversionCommands.ToGrammarBuilder());
            SemanticResultKey srkActivateConversion = new SemanticResultKey("conversion command", new GrammarBuilder("computer convert "));

            //Unit conversion grammar
            Choices conversionChoices = new Choices();
            conversionChoices.Add(" centimeters into inches");
            conversionChoices.Add(" inches into centimeters");
            SemanticResultKey srkIntoUnits = new SemanticResultKey("conversion choices", conversionChoices.ToGrammarBuilder());

            GrammarBuilder unitConversionGrammarBuilder = new GrammarBuilder();
            unitConversionGrammarBuilder.Culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            unitConversionGrammarBuilder.Append(srkActivateConversion);
            unitConversionGrammarBuilder.AppendDictation();
            unitConversionGrammarBuilder.Append(srkIntoUnits);


            //Google grammar
            SemanticResultKey srkGoogle = new SemanticResultKey("google", new GrammarBuilder("computer google "));

            GrammarBuilder googleGrammarBuilder = new GrammarBuilder();
            googleGrammarBuilder.Append(srkGoogle);
            googleGrammarBuilder.AppendDictation();

            try
            {

                recognizeCommands.RequestRecognizerUpdate();
                recognizeCommands.LoadGrammar(new Grammar(unitConversionGrammarBuilder));
                recognizeCommands.LoadGrammar(new Grammar(googleGrammarBuilder));
                recognizeCommands.LoadGrammar(gr);
                //recognizeCommands.LoadGrammar(defaultDictationGrammar);

            }

            catch
            {
                return;
            }
        }

        void recognizeCommands_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            //micListeningBox.BackColor = Color.Red;
        }


        public double convertUnits(double unit, string unitFrom, string unitTo)
        {
            double rc = 0;


            if (unitTo.Contains("inch") && unitFrom.Contains("centimeter"))
            {
                rc = Convert.ToDouble(unit * 0.39370);
            }
            else if (unitFrom.Contains("inch") && unitTo.Contains("centimeter"))
            {
                rc = Convert.ToDouble(unit / 0.39370);
            }


            return Math.Round(rc, 2);

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
            int audioLevel = (int)(Math.Log((e.AudioLevel)) * 75);

            if (audioLevel > 255)
            {
                audioLevel = 255;
            }
            else if (audioLevel < 0)
            {
                audioLevel = 0;
            }
            micListeningBox.BackColor = Color.FromArgb(audioLevel, 0, 0);


        }

        private void recognizeCommands_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "exit")
            {
                Application.Exit();
            }

            else if (e.Result.Text == "computer shutdown")
            {
                shutdownPC();
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
                            speak("turning lights off");
                            break;
                        case "roomy lights on":
                            hueBridge.turnLightsOn("0");
                            tbConsoleOutput.AppendText("lights are on" + System.Environment.NewLine);
                            speak("turning lights on");
                            break;
                        case "roomy red lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 0, 0), "0");
                            tbConsoleOutput.AppendText("switched to red lights" + System.Environment.NewLine);
                            speak("setting red lights");
                            break;
                        case "roomy green lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 255, 0), "0");
                            tbConsoleOutput.AppendText("switched to green lights" + System.Environment.NewLine);
                            speak("setting green lights");
                            break;
                        case "roomy blue lights":
                            hueBridge.changeLightColor(Color.FromArgb(0, 0, 255), "0");
                            tbConsoleOutput.AppendText("switched to blue lights" + System.Environment.NewLine);
                            speak("setting blue lights");
                            break;
                        case "roomy yellow lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 0), "0");
                            tbConsoleOutput.AppendText("switched to yellow lights" + System.Environment.NewLine);
                            speak("setting yellow lights");
                            break;
                        case "roomy orange lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 128, 0), "0");
                            tbConsoleOutput.AppendText("switched to orange lights" + System.Environment.NewLine);
                            speak("setting orange lights");
                            break;
                        case "roomy white lights":
                            hueBridge.changeLightColor(Color.FromArgb(255, 255, 255), "0");
                            tbConsoleOutput.AppendText("switched to white lights" + System.Environment.NewLine);
                            speak("setting white lights");
                            break;
                        case "roomy purple lights":
                            hueBridge.changeLightColor(Color.FromArgb(127, 0, 255), "0");
                            tbConsoleOutput.AppendText("switched to purple lights" + System.Environment.NewLine);
                            speak("setting purple lights");
                            break;
                        default:
                            //tbConsoleOutput.AppendText(e.Result.Text.ToString());
                            break;

                    }

                    if (command.Contains("computer"))
                    {
                        if (command.Contains("computer google"))
                        {

                            launchGoogle(command.Replace("computer google ", ""));
                        }

                        else if (command.Contains("inch") || command.Contains("centimeter"))
                        {
                            double parsed = ParseEnglishNumbers(command);
                            double result = convertUnits(parsed, e.Result.Words[e.Result.Words.Count - 3].Text, e.Result.Words[e.Result.Words.Count - 1].Text);
                            speak(parsed + " " + e.Result.Words[e.Result.Words.Count - 3].Text + " is " + result.ToString() + " " + e.Result.Words[e.Result.Words.Count - 1].Text);
                            string resultOutput = parsed + " " + e.Result.Words[e.Result.Words.Count - 3].Text + " is " + result.ToString() + " " + e.Result.Words[e.Result.Words.Count - 1].Text + System.Environment.NewLine;
                            tbConsoleOutput.AppendText(resultOutput);

                        }
                        else if (command == "computer open excel")
                        {
                            speak("Opening Excel");
                            openExcel();
                        }
                        else if (command == "computer open word")
                        {
                            speak("Opening Word");
                            openWord();
                        }
                        else if (command == "computer start tf2")
                        {
                            speak("Opening TF2");

                            startTF2();
                        }
                        else if (command == "computer open you tube")
                        {
                            System.Diagnostics.Process.Start("http://www.youtube.com");
                        }
                        else if (command == "computer check mail")
                        {
                            System.Diagnostics.Process.Start("http://www.gmail.com");
                        }

                    }


                    if (command.Contains("%") && command.Contains("roomy"))
                    {
                        string[] dimPercentage = command.Split(' ');
                        hueBridge.setBrightness(dimPercentage[2], "0");
                       speak("setting lights to" + dimPercentage[2].ToString());
                    }
                }

            }

            tbVoiceOutput.AppendText(e.Result.Text.ToString() + System.Environment.NewLine);
            /*
            textBox3.Text = "";
            for (int i = 0; i < e.Result.Alternates.Count; i++)
            {

                textBox3.AppendText(e.Result.Alternates[i].Text + System.Environment.NewLine);
            }
             */
            //micListeningBox.BackColor = Color.DimGray;

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

        private void shutdownPC()
        {
            shutdownForm = new HueShutdownForm(this, sSynth);
            recognizeCommands.RecognizeAsyncStop();
            this.Enabled = false;
            micListeningBox.BackColor = Color.Gray;
            shutdownForm.FormClosed += shutdown_FormClosed;
            shutdownForm.ShowDialog(this);


        }

        void shutdown_FormClosed(object sender, FormClosedEventArgs e)
        {

            this.Enabled = true;
            micListeningBox.BackColor = Color.Black;
            recognizeCommands.RecognizeAsync(RecognizeMode.Multiple);

        }

        /*
        private Grammar CreateUnitGrammer()
        {
            // Create a set of color choices.
            Choices colorChoice = new Choices(new string[] { "red", "green", "blue" });

            // Create grammar builders for the two versions of the phrase.
            GrammarBuilder makePhrase =
              GrammarBuilder.Add((GrammarBuilder)"Make background", colorChoice);
            GrammarBuilder setPhrase =
              GrammarBuilder.Add("Set background to", (GrammarBuilder)colorChoice);

            // Create a Choices for the two alternative phrases, convert the Choices
            // to a GrammarBuilder, and construct the grammar from the result.
            Choices bothChoices = new Choices(new GrammarBuilder[] { makePhrase, setPhrase });
            GrammarBuilder bothPhrases = new GrammarBuilder(bothChoices);

            Grammar grammar = new Grammar(bothPhrases);
            grammar.Name = "backgroundColor";
            return grammar;
        }
         */

        /*
        private Grammar[] CreateSubsetMatchTest()
        {
            List<Grammar> grammars = new List<Grammar>(4);

            string phrase = "one two three four five six";
            foreach (SubsetMatchingMode mode in
              Enum.GetValues(typeof(SubsetMatchingMode)))
            {
                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(phrase, mode);

                Grammar grammar = new Grammar(gb);
                grammar.Name = mode.ToString();
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }
         */

        public void openExcel()
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            Excel.Workbook newWorkbook = excelApp.Workbooks.Add();
        }
        public void openWord()
        {
            Word.Application wordApp = new Word.Application();
            wordApp.Visible = true;
            Word.Document newDocument = wordApp.Documents.Add();

        }

        public void startTF2()
        {
            Process.Start("steam://rungameid/440");
        }

        private void launchGoogle(string term)
        {

            Process.Start("http://google.com/search?q=" + term);
        }


        static double ParseEnglishNumbers(string number)
        {
            string[] words = number.ToLower().Split(new char[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            Dictionary<string, int> modifiers = new Dictionary<string, int>() {
                {"billion", 1000000000},
                {"million", 1000000},
                {"thousand", 1000},
                {"hundred", 100}
            };

            if (number == "eleventy billion")
                return int.MaxValue; // 110,000,000,000 is out of range for an int!

            double result = 0;
            double currentResult = 0;
            double lastModifier = 1;
            double decimalPlace = 0;

            foreach (string word in words)
            {

                if (modifiers.ContainsKey(word))
                {
                    lastModifier *= modifiers[word];
                }
                else
                {
                    double n;

                    if (lastModifier > 1)
                    {
                        result += currentResult * lastModifier;
                        lastModifier = 1;
                        currentResult = 0;
                    }

                    if ((n = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        if (decimalPlace > 0)
                        {
                            currentResult += (n / decimalPlace);
                            decimalPlace *= 10;
                        }
                        else
                        {
                            currentResult += n;
                        }
                    }
                    else if ((n = Array.IndexOf(teens, word) + 1) > 0)
                    {
                        if (decimalPlace > 0)
                        {
                            currentResult += ((n + 10) / decimalPlace);
                            decimalPlace *= 10;
                        }
                        else
                        {
                            currentResult += n + 10;
                        }

                    }
                    else if ((n = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        if (decimalPlace > 0)
                        {
                            currentResult += ((n * 10) / decimalPlace);
                            decimalPlace *= 10;
                        }
                        else
                        {
                            currentResult += n * 10;
                        }
                    }
                    else if (word == "point")
                    {
                        decimalPlace = 10;
                    }
                    else if (word != "and")
                    {
                        //throw new ApplicationException("Unrecognized word: " + word);
                    }

                }

            }

            return result + currentResult * lastModifier;
        }

        private void drawSynthVolumeUI()
        {
            var cntrls = new List<RoundButton>();
            int spaceBetweenLayers = 5;
            int x = 200, y = 200;


            for (int i = 1; i <= 25; i++)
            {
                int circleRadius = (spaceBetweenLayers + i) * 10;

                this.Controls.Add(new RoundButton()
                {
                    Name = "synthVolume" + i,
                    BackColor = Color.Black,
                    Location = new System.Drawing.Point(x - circleRadius / 2, y - circleRadius / 2),
                    Size = new System.Drawing.Size(circleRadius, circleRadius),
                    Enabled = false

                });



            }


        }
        //getting volume for current default device
        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            MMDeviceEnumerator de = new MMDeviceEnumerator();
            MMDevice device = de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            float volume = (float)device.AudioMeterInformation.MasterPeakValue * 100;
            progressBar1.Value = (int)volume;
        }
        */

        private void speak(string phrase)
        {
            // Configure the audio output. 
            sSynth.SetOutputToWaveFile(@"C:\temp\test.wav", new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
            MemoryStream stream = new MemoryStream();


            // Build a prompt.
            PromptBuilder builder = new PromptBuilder();
            builder.AppendText(phrase);

            // Speak the prompt.
            sSynth.Speak(builder);

            sSynth.SetOutputToNull();
            sSynth.SetOutputToDefaultAudioDevice();


            player = new WaveOut();
            wavFile = new AudioFileReader(@"C:\temp\test.wav");
            var meter = new MeteringSampleProvider(wavFile);
            meter.StreamVolume += onPostVolumeMeter;
            player.DesiredLatency = 70;
            player.Init(new SampleToWaveProvider(meter));
            player.PlaybackStopped += player_PlaybackStopped;
            player.Play();
            
        }

    }

}
