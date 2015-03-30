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
using Test_2;

namespace KeyTrackerBase
{
    public partial class emailForgot : Form
    {
        string[] settingsLoad = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt");

        public emailForgot()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            if (emailBox.Text == settingsLoad[0])
            {
                string old = Settings.EncryptDecrypt(settingsLoad[3], 3);
                Form1.SendEmail(settingsLoad[0], "Forgotten Password? User: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + " Requested Password", "IF YOU HAVE NOT REQUESTED YOU PASSWORD CHECK THE LOG FOR THIS USER\n\n\nPassword: " + old, null);
                MessageBox.Show("Password sent to email address");
                this.Close();
            }
            else
                MessageBox.Show("Incorrect Email Address");
        }
    }
}
