using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Test_2
{
    public partial class Settings : Form
    {
        string[] settingsLoad = File.ReadAllLines(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\settings.txt");
        string[] settingsSave = new string[6];

        public Settings()
        {
            InitializeComponent();
        }

        //LOAD CURRENT SETTINGS
        private void Settings_Load(object sender, EventArgs e)
        {
            adminEmail.Text = settingsLoad[0];
            onOff.Text = settingsLoad[1];
            format.Text = settingsLoad[2];
            level.Text = settingsLoad[4];
            location.Text = settingsLoad[5];
        }

        //SET PASSWORD
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //SAVE
        private void button3_Click(object sender, EventArgs e)
        {
            //USER SETTING PREFERENCES:
            settingsSave[0] = adminEmail.Text;//email
            settingsSave[1] = onOff.Text;//switch (on/off)
            settingsSave[2] = format.Text;//picture format
            settingsSave[3] = settingsLoad[3];
            settingsSave[4] = level.Text;//detection level
            settingsSave[5] = location.Text;//location

            //Writes array to .txt file
            File.WriteAllLines(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\settings.txt", settingsSave);

            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }
    }
}
