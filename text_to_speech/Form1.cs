using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.IO;

namespace text_to_speech
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer reader;
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            reader = new SpeechSynthesizer();
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            textBox1.ScrollBars = ScrollBars.Both;
        }

        //SPEAK TEXT
        private void button1_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                reader.Dispose();
            }
            reader = null;
            reader = new SpeechSynthesizer();
            if (textBox1.Text != "")
            {

                reader = new SpeechSynthesizer();
                reader.SpeakAsync(textBox1.Text);
                label2.Text = "SPEAKING";

                button2.Enabled = true;
                button4.Enabled = true;
                 

                reader.Volume = tbVolume.Value;  
                reader.Rate = tbSpeed.Value; 

                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
            else
            {
                MessageBox.Show("Please enter some text in the textbox", "Message", MessageBoxButtons.OK);
            }
        }

        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            label2.Text = "IDLE";
        }

        //PAUSE
        private void button2_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Speaking)
                {
                    reader.Pause();
                    label2.Text = "PAUSED";
                    button3.Enabled = true;

                }
            }
        }

        //RESUME
        private void button3_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                if (reader.State == SynthesizerState.Paused)
                {

                    reader.Volume = tbVolume.Value;
                    reader.Rate = tbSpeed.Value; 
                    reader.Resume();
                    label2.Text = "SPEAKING";
                }
                button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (reader != null)
            {
                reader.Dispose();
                label2.Text = "IDLE";
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                reader = null;
                 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = File.ReadAllText(openFileDialog1.FileName.ToString(), GetFileEncodeType(openFileDialog1.FileName.ToString()));

        }


        public System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }

        private void tbSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (reader != null)
            {
             
                reader.Rate = tbSpeed.Value;
            }
        }

        private void tbVolume_ValueChanged(object sender, EventArgs e)
        {
            if (reader != null)
            {
                reader.Volume = tbVolume.Value;
            }
        }

        

      



    }
}
