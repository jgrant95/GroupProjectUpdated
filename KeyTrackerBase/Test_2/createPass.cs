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
    public partial class createPass : Form
    {
        string[] settingsSave = new string[6];

        public createPass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {
                    if (comboBox1.Text != "")
                    {
                        if (comboBox2.Text != "")
                        {
                            if (comboBox3.Text != "")
                            {
                                if (comboBox4.Text != "")
                                {

                                    string old = Settings.EncryptDecrypt(textBox2.Text, 3);

                                    //saves data to txt file accordingly -array
                                    settingsSave[0] = textBox1.Text;
                                    settingsSave[1] = comboBox4.Text;
                                    settingsSave[2] = comboBox1.Text;
                                    settingsSave[3] = old;
                                    settingsSave[4] = comboBox2.Text;
                                    settingsSave[5] = comboBox3.Text;

                                    File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt", settingsSave);

                                    //confirmation message
                                    MessageBox.Show("Settings Saved! Below are your confirmed settings:\n\n" + settingsSave[0] + "\n" + settingsSave[1] + "\n" + settingsSave[2] + "\n" + textBox2.Text + "\n" + settingsSave[4] + "\n" + settingsSave[5] + "\n\nYou can change these settings(if required) in the settings of the program (right click on icon in system tray)\n\nTO BEGIN USING THE ANTI-BULLYING SOFTWARE, RE-OPEN THE APPLICATION!");

                                    this.Close();

                                    Properties.Settings.Default.IsFirstTime = false;
                                    Properties.Settings.Default.Save();

                                }
                                else
                                    MessageBox.Show("Please Select To Either Have The Program On Or Off When It First Launches");
                            }
                            else
                                MessageBox.Show("Please Select A Location");
                        }
                        else
                            MessageBox.Show("Please Select A Word Detection Level");
                    }
                    else
                        MessageBox.Show("Please Select A Picture Format");
                }
                else
                    MessageBox.Show("Please Enter A Password");
            }
            else
                MessageBox.Show("Please Enter An Email Address");

            
                
            
        }
    }
}
