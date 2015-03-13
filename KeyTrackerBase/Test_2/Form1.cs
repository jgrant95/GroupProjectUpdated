using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
//using System.Windows.Threading;
using System.Windows.Forms;
using KeyTrackerBase;

namespace KeyTrackerBase
{
    public partial class Form1 : Form
    {
        bool shift = false;
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

            #region Key Handling
            e.Handled = false;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                shift = true;//turns shift on for next char

            if (e.KeyCode == Keys.Back)
            {
                if (textBox1.Text != "")
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                e.Handled = false;
            }


            //finds out to be lower or upper case
            if (e.KeyCode != Keys.Back)//ensures backspace isnt entered into string
            {
                if (shift == false)
                {
                    if (e.KeyCode != Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                        textBox1.Text += ((char)e.KeyValue).ToString().ToLower();
                }
                else if (e.KeyCode != Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                {
                    textBox1.Text += ((char)e.KeyValue).ToString().ToUpper();
                    shift = false;
                }
            }
            if (e.KeyCode == Keys.Return)
                textBox1.Text += "\n";
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
