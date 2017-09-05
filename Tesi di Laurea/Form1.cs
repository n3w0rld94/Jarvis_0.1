using System;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
namespace Tesi_di_Laurea
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer Jarvis_talk = new SpeechSynthesizer();
        PromptBuilder pbuilder = new PromptBuilder();
        SpeechRecognitionEngine Jarvis_read = new SpeechRecognitionEngine();
        string name = "User";
        public Form1()
        {
            InitializeComponent();
            
            //Jarvis_talk.Speak("Greetings Mister" + name + ", my name is Jarvis and my goal is to satisfy you in whichever way you desire. To stop the recognition, say Goodby Jarvis. Now, let's start by loading a Dictionary, and then tell me your name");
        }

        string[] Dictionary = null;
        Choices lBox = new Choices();

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (Dictionary != null)
            {
                lBox.Add(Dictionary);
                Jarvis_read.SpeechRecognized += Jarvis_read_SpeechRecognized;
                btn_start.Enabled = false;
                btn_stop.Enabled = true;
                Grammar grammar = new Grammar(new GrammarBuilder(lBox));
                try
                {
                    Jarvis_read.RequestRecognizerUpdate();
                    Jarvis_read.LoadGrammarAsync(grammar);

                    Jarvis_read.SetInputToDefaultAudioDevice();
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERRORE");
                    return;
                }
            }
            else
                Jarvis_talk.Speak("You must select a dictionary before starting recognition");
        } 

        private void Jarvis_read_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text.ToString()) {
                case "How are you":
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Better than good! Thanks");
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "Hi Jarvis":
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Yes sir " + name);
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "Find oreos for me":
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Maybe you should look for them in the kitchen " + name + " oh lord of the galaxy");
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "Goodbye Jarvis":
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Goodbye master " + name);
                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;
                    name = "user";
                    break;
                case "Open Opera":
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Done master");
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "Tell me a story":
                    string b;
                    StreamReader F = File.OpenText("C:/Users/Shea/Documents/visual studio 2015/Projects/Tesi di Laurea/Tesi di Laurea/Stories.txt");
                    if (F == null)
                        Jarvis_talk.Speak("I have no story to tell you");
                    else {
                        b = F.ReadLine();
                        Jarvis_talk.Speak("Sure master " + name + b);
                        b = F.ReadLine();
                        while ((b != "\n") && (b != null)) {
                            b = F.ReadLine();
                            Jarvis_read.RecognizeAsyncStop();
                            Jarvis_talk.Speak(b);
                            Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                        }
                    }
                    break;
                case "Ilyas":
                    name = e.Result.Text.ToString();
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Welcome back " + name);
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "Morad":
                    name = e.Result.Text.ToString();
                    Jarvis_read.RecognizeAsyncStop();
                    Jarvis_talk.Speak("Welcome back " + name);
                    Jarvis_read.RecognizeAsync(RecognizeMode.Multiple);
                    break;

            }
            TextBox.AppendText(e.Result.Text.ToString() + " ");
        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            pbuilder.ClearContent();
            pbuilder.AppendText(TextBox.Text);
            Jarvis_talk.Speak(pbuilder);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Jarvis_read.RecognizeAsyncStop();
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Dictionary = File.ReadAllLines(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Errore");
                }
            }
            
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
