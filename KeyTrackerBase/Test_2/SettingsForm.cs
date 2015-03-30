using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using KeyTrackerBase;

namespace Test_2
{
    public partial class Settings : Form
    {
        string final;
        string[] settingsLoad = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt");
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

        //SET PASSWORD - Key:3
        private void button2_Click(object sender, EventArgs e)
        {
            //decrypts old password stored
            string old = EncryptDecrypt(settingsLoad[3], 3);
            
            //ensures textbox isnt empty
            if (textBox3.Text != "")
            {
                //ensures old password is equal to one stored in txt file
                if (textBox2.Text == old)
                {
                    //uses encryption function to encrypt the new password
                    final = EncryptDecrypt(textBox3.Text, 3);

                    //saves to array
                    saveAll(final);

                    //Writes array to .txt file
                    File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt", settingsSave);
                    settingsLoad = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt");
                    MessageBox.Show("Password Has Been Changed!");
                }
                else
                {
                    MessageBox.Show("Old Password Incorrect!");
                }
            }
            else
            {
                MessageBox.Show("Please Enter a New Password");
            }
        }
        
        //XOR ENCRYPTION
        public static string EncryptDecrypt(string textToEncrypt, int encryptionKey)
        {
            //creates new string with no more than original strings capacity
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            //creates new string of the same length of the text file - this will be used for the encrypting
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);

            char c;

            //loops through input text
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                //takes each original char one by one and puts them to the power of the encryption key (xor)
                c = inSb[i];
                c = (char)(c ^ encryptionKey);
                //placed into second string
                outSb.Append(c);
            }
            return outSb.ToString();
        }

        //SAVE
        private void button3_Click(object sender, EventArgs e)
        {
            saveAll(settingsLoad[3]);

            //Writes array to .txt file
            File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt", settingsSave);

            //restarts            
            Application.Exit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        public void saveAll (string toSave)
        {
            //USER SETTING PREFERENCES:
            settingsSave[0] = adminEmail.Text;//email
            settingsSave[1] = onOff.Text;//switch (on/off)
            settingsSave[2] = format.Text;//picture format
            settingsSave[3] = toSave;//used for code efficiency for saving password
            settingsSave[4] = level.Text;//detection level
            settingsSave[5] = location.Text;//location
        }
    }
}
