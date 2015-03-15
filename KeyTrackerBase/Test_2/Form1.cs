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

namespace KeyTrackerBase
{
    public partial class Form1 : Form
    {
        //bool keys
        bool shift = false;

        //string word = "cunt";
        string sentence;
        int countWord = 0;
        int countChar = 0;

        public Form1()
        {
            

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

            #region Key Entry Logger (Main)
            //ensures backspace isnt entered into string
            if (e.KeyCode != Keys.Back)
            {
                sentence += ((char)e.KeyValue).ToString().ToLower();
                textBox1.Text = sentence;
                //ends sentence and logs when word count hits 30, character count hits 180, full stop is entered or the return key is entered
                if (countWord == 30 || countChar == 180 || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Return)
                    {//asks, is string not empty? & is the string not have \r (what the return key produces)
                        if (sentence != "" && sentence != "\r")
                        {
                            using (StreamWriter file = new StreamWriter(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\log.txt", true))
                            {
                                //writes entries to txt file - logger
                                file.WriteLine(DateTime.Now.ToString() + ":  " + sentence);
                                //string and ints reset
                                sentence = "";
                                countWord = 0;
                                countChar = 0;
                                file.Close();
                            }
                            
                        }
                        else
                            //ensures the 'returns' arent kept in txt file
                            sentence = "";





                    //StreamReader file = new StreamReader("C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\log.txt");
                    //while((word = file.ReadLine()) != null)
                    //{
                    //    if (textBox1.Text == word)
                    //        MessageBox.Show("CUNT ALERT!");
                    //}
                    //countWord++;
                    //file.Close();
                }
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
