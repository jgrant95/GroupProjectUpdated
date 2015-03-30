using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyTrackerBase
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //determines whether to run get started form or normal, uses settings function to do so and save this
            if (Properties.Settings.Default.IsFirstTime == true)
            {
                Application.Run(new createPass());
            }
            else
                Application.Run(new Form1());

        }
    }
}
