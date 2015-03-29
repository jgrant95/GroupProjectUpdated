﻿using System;
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
        string[] settingsLoad = File.ReadAllLines(@"C:\\Users\\Jon\\Desktop\\GroupProjectUpdated\\KeyTrackerBase\\settings.txt");

        public Password()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            //decrypts old password stored - uses encryption used in settings form
            string old = Settings.EncryptDecrypt(settingsLoad[3], 3);
            
            //ensures textbox isnt empty
            if (passBox.Text != "")
            {
                //ensures old password is equal to one stored in txt file
                if (passBox.Text == old)
                {
                    this.Close();
                    Settings settingsForm = new Settings();
                    settingsForm.Show();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            emailForgot emailForm = new emailForgot();
            emailForm.Show();
        }

    }
}
