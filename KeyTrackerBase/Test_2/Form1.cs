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
        public Form1()
        {
            InitializeComponent();

            #region Keys to watch
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            //automatically detects keys inputted for use in KeyDown event below
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gkh.HookedKeys.Add(key);

            #endregion

        }

        //new instance of glabal key hook
        globalKeyboardHook gkh = new globalKeyboardHook();

        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            RoutedEventArgs f = new RoutedEventArgs();

            
            e.Handled = false;
            {
                textBox1.Text += ((char)e.KeyValue).ToString().ToLower();
            }

        }
    }
}
