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
    public partial class Password : Form
    {
        string[] settingsLoad = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt");

        public Password()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            //decrypts old password stored - uses encryption used in settings form
            string old = Settings.EncryptDecrypt(settingsLoad[3], 3);
         
            if (old != "")
            {
                //ensures textbox isnt empty
                if (passBox.Text != "")
                {
                    //ensures old password is equal to one stored in txt file
                    if (passBox.Text == old)
                    {
                        if (Properties.Settings.Default.passwordDetermine == "settings")
                        {
                            Settings settingsForm = new Settings();
                            this.Close();
                            settingsForm.Show();
                        }
                        else if (Properties.Settings.Default.passwordDetermine == "quit")
                        {
                            this.Close();
                            Application.Exit();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password Incorrect!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Password");
                }
            }
            else
                MessageBox.Show("Please contact technical support regarding:\n\nERROR04 - Settings - Password NULL");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            emailForgot emailForm = new emailForgot();
            emailForm.Show();
            
        }

    }
}
