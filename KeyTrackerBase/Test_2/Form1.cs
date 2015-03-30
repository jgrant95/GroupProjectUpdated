using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using KeyTrackerBase;
using System.Drawing;
using System.Net.Mail;//used for emailing
using Test_2;
using Ionic.Zip;

//cyberbullying

namespace KeyTrackerBase
{
    public partial class Form1 : Form
    {
        string sentence;
        string badWords;
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        int countWord = 0;
        int countChar = 0;
        int countDetect = 0;
        int iScreen = 0;

        //reads .txt files into arrays
       // string[] words;
        string[] words;
        string[] low = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\low.txt");
        string[] medium = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\medium.txt");
        string[] high = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\high.txt");
        string[] settings = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings.txt");

        //create icon 
        NotifyIcon systemTrayIcon;
        Icon applicationIcon;

        public Form1()
        {
            ////revert to default - get started
            //Properties.Settings.Default.IsFirstTime = true;
            //Properties.Settings.Default.Save();

            #region Form & System Tray
            //hide form
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Load icon
            switch (settings[1])
            {
                case "On":
                    applicationIcon = new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\f_Owl_Icon.ico");
                    systemTray("Quit", "AntiBullying Prototype v0.5", "Settings");
                    break;
                case "Off":
                    applicationIcon = new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\f_Owl_Icon_off.ico");
                    systemTray("Quit", "AntiBullying Prototype v0.5 - Disabled", "Settings");
                    break;
                default:
                    MessageBox.Show("Please contact technical support regarding:\n\nERROR01 - Settings - Invalid switch option");
                    break;
            }

            InitializeComponent();
            #endregion

            #region Keys to watch
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new System.Windows.Forms.KeyEventHandler(gkh_KeyUp);
            //automatically detects keys inputted for use in KeyDown event below
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gkh.HookedKeys.Add(key);

            #endregion

            #region Word Checker - Level

            //array to use for combined arrays (for different levels)
            

            //hierarchy level of words
            switch (settings[4])
            {
                case "All (Recommended)":
                    words =  new string[low.Length + medium.Length + high.Length];
                    low.CopyTo(words, 0);//adds a copy of the low array to the beginning of the new array
                    medium.CopyTo(words, low.Length);//same but at the end of the length of low
                    high.CopyTo(words, low.Length + medium.Length);//same but end of the length of low and medium
                    break;
                case "High/Medium":
                    words =  new string[low.Length + medium.Length + high.Length];
                    medium.CopyTo(words, 0);//adds a copy of the low array to the beginning of the new array
                    high.CopyTo(words, medium.Length);//same but at the end of the length of low
                    break;
                case "Medium/Low":
                    words =  new string[low.Length + medium.Length + high.Length];
                    medium.CopyTo(words, 0);//adds a copy of the low array to the beginning of the new array
                    high.CopyTo(words, medium.Length);//same but at the end of the length of low
                    break;
                default:
                    MessageBox.Show("Please contact technical support regarding:\n\nERROR02 - Settings - Invalid level detection option");
                    break;
            }

            #endregion

        }

        public void systemTray(string quit, string name, string settings)
        {
            //show system tray icon and assign the icon for it
            systemTrayIcon = new NotifyIcon();
            systemTrayIcon.Icon = applicationIcon;
            systemTrayIcon.Visible = true;

            //creates the menu in the system tray icon
            MenuItem quitMenuItem = new MenuItem(quit);
            MenuItem programName = new MenuItem(name);
            MenuItem settingsItem = new MenuItem(settings);
            ContextMenu systemTrayMenu = new ContextMenu();
            systemTrayMenu.MenuItems.Add(programName);
            systemTrayMenu.MenuItems.Add(settingsItem);
            systemTrayMenu.MenuItems.Add(quitMenuItem);
            systemTrayIcon.ContextMenu = systemTrayMenu;

            //creates events for the specific menu items
            quitMenuItem.Click += quitMenuItem_Click;
            settingsItem.Click += settingsItem_Click;
        }

        //opens settings form on click in menu
        void settingsItem_Click(object sender, EventArgs e)
        {
            Password passwordForm = new Password();
            passwordForm.Show();
        }

        //quits the program
        public void quitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //new instance of glabal key hook
        globalKeyboardHook gkh = new globalKeyboardHook();

        private void KeyPressMod(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //validates if the program is turned on via. settings
            if (settings[1] == "On")
            {
                RoutedEventArgs f = new RoutedEventArgs();

                //counts characters
                countChar++;

                //DEFINE CERTAIN KEYS 
                #region Spacebar
                if (e.KeyCode == Keys.Space)
                    countWord++;
                #endregion

                #region Backspace
                if (e.KeyCode == Keys.Back)
                {
                    if (sentence != "")
                    {
                        sentence = sentence.Remove(sentence.Length - 1);
                        e.Handled = false;
                    }
                }
                #endregion
                //MIGHT BE WORTH ADDING A VARYING TIMER TO RECORD TEXT IN THE PAST _ MINUTES
                #region New Key Logger (Main) - User

                if (e.KeyValue != 160)//ensures 'shift' isnt entered as a key
                {
                    if (e.KeyCode != Keys.Back)//ensures no 'backspaces'
                    {
                        if (e.KeyValue != 188)//ensures no 'commars'
                        {
                            if (e.Shift != true && e.KeyValue != 49)//exclamation
                            {
                                if (e.Shift != true && e.KeyValue != 191)//question mark
                                {
                                    sentence += ((char)e.KeyValue).ToString().ToLower();

                                    if (sentence != "\r")//ensures no 'returns' are by themself
                                    {
                                        if (sentence != "¾")//ensures no 'fullstops' are by themself
                                        {
                                            if (countWord == 30 || countChar == 180 || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Return)
                                            {
                                                if (sentence != "" || e.Shift != true)
                                                {
                                                    //loads the log file to write into
                                                        using (StreamWriter file = new StreamWriter(Path.GetDirectoryName(Application.ExecutablePath) + "\\log.txt", true))
                                                        {

                                                            if (e.KeyCode != Keys.OemPeriod)
                                                            {

                                                                //writes entries to txt file - logger
                                                                file.WriteLine(DateTime.Now.ToString() + ":  " + sentence);
                                                                //calls function to detect for words
                                                                wordDetector(file);

                                                            }

                                                            //FULL STOP (PERIOD) LOGGING
                                                            else if (e.KeyCode == Keys.OemPeriod)
                                                            {

                                                                //writes entries to txt file, removes period symbol and puts fullstop - logger
                                                                file.WriteLine(DateTime.Now.ToString() + ":  " + sentence.Remove(sentence.Length - 1) + ".");
                                                                wordDetector(file);

                                                            }

                                                            //string and ints reset
                                                            sentence = "";
                                                            countWord = 0;
                                                            countChar = 0;
                                                            countDetect = 0;
                                                            badWords = "";
                                                            file.Close();
                                                        }
                                                    }
                                            }
                                        }
                                        else
                                            sentence = "";
                                    }
                                    else
                                        sentence = "";
                                }
                                else
                                    sentence += "?";
                            }
                            else
                                sentence += "!";
                        }
                        else
                            sentence += ",";
                    }
                }
                #endregion
            }
            else
            {

            }
        }


        void gkh_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Put keyup events in here (if necessary)
            //DELETE IF UNUSED - ALSO IN GLOBALKEYHOOK!!!
        }

        //WORD DETECTION WORDING PERFECTLY!
        public void wordDetector(StreamWriter file)
        {
            //word detection begin - adds the words and the amount of words found in sentence to vars
            for (int i = 0; i < words.Length; i++)
            {
                if (sentence.Contains(words[i]))
                {
                    countDetect++;
                    badWords += words[i] + " ";
                }

            }

            //SENDS WORDS AND SCREENSHOT TO EMAIL IF THE AMOUNT DETECTED IS > THAN 0
            if (countDetect > 0)
            {
                file.WriteLine("Amount of Words Detected: " + countDetect + " - Words: " + badWords);
                //capture screenshot & send email with it and log message
                //uses chosen format for screenshot

                    Bitmap screencapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    Graphics graphics = Graphics.FromImage(screencapture as Image);
                    switch (settings[2])
                    {
                        case "JPEG":
                            graphics.CopyFromScreen(0, 0, 0, 0, screencapture.Size);
                            screencapture.Save(Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshot" + iScreen + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case "PNG":
                            graphics.CopyFromScreen(0, 0, 0, 0, screencapture.Size);
                            screencapture.Save(Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshot" + iScreen + ".png", System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case "GIF":
                            graphics.CopyFromScreen(0, 0, 0, 0, screencapture.Size);
                            screencapture.Save(Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshot" + iScreen + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        default:
                            MessageBox.Show("Please contact technical support regarding:\n\nERROR03 - Settings - Invalid screenshot format option");
                            break;
                    }

                    #region Delete Screenshots
                    if (iScreen >= 20)
                    {
                        for (int i = 0; i <= iScreen; i++)
                        {
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshot" + i + "." + settings[2].ToLower()))
                            {
                                try
                                {
                                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshot" + i + "." + settings[2].ToLower());
                                }
                                catch (System.IO.IOException e)
                                {
                                    MessageBox.Show(e + "\nPlease contact technical support regarding:\n\nERROR06 - SCREENSHOTS - Cannot delete screenshots");
                                }
                            }
                        }
                        iScreen = -1;
                    }
                    #endregion

                    SendEmail(settings[0], "User: " + userName + " " + badWords, "Sentence recorded at " + DateTime.Now.ToString() + ": " + sentence, @"C:\\Users\\Jon\\Desktop\\screenshot" + iScreen + settings[2].ToLower());
                    iScreen++;

                }
        }

        //EMAIL WORKING PERFECTLY!
        public static void SendEmail(string to, string subject, string body, string path)
        {
            if (to != "")
            {
                try
                {
                    //------------------------------------Email message code-------------------------------

                    //creating the email object

                    MailMessage message = new MailMessage("screenshotbully@gmail.com", to);
                    //Body or content of the email indicating a new bullying incident 
                    message.Body = body;//example
                    //subject or title of the email
                    message.Subject = (subject);//gets username currently logged in
                    //attachment code of the screenshot, direct acess to file path of the screenshot or it can be replaced by a variable
                    if (path != null)
                    {
                        message.Attachments.Add(new Attachment(path));
                    }
                    //------------------------------end----------------------------------------------

                    //------------------------------Email client used (gmail)------------------------------

                    //crearting client object with gmail smtp details and port
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.ServicePoint.MaxIdleTime = 1;
                    //login credentials of screenshot sender
                    client.Credentials = new System.Net.NetworkCredential("screenshotbully@gmail.com", "cyberbullying");
                    //enabling secure connection
                    client.EnableSsl = true;
                    //sending the email message - using 'SendAsync' as it almost completley removes lag
                    client.SendAsync(message, null);

                    //------------------------------end-----------------------------------------------
                    //freeing memory
                    message = null;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error Email");
                    throw Ex;
                }
            }
            else
                MessageBox.Show("Please contact technical support regarding:\n\nERROR05 - Settings - Email address NULL");
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            systemTrayIcon.Visible = false;
        }

    }
}
