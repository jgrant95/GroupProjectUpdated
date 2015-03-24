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

//cyberbullying

namespace KeyTrackerBase
{
    public partial class Form1 : Form
    {
        string sentence;
        string badWords;
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string[] words = File.ReadAllLines(@"C:\\Users\\Dave\\Documents\\GitHub\\GroupProjectUpdated\\KeyTrackerBase\\words.txt");
        int countWord = 0;
        int countChar = 0;
        int countDetect = 0;
        int iScreen = 0;

        //create icon 
        NotifyIcon systemTrayIcon;
        Icon applicationIcon;

        public Form1()
        {
            #region Form Stuff
            //hide form
            this.WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;

            //Load icon
            applicationIcon = new Icon("C:\\Users\\Dave\\Documents\\GitHub\\GroupProjectUpdated\\KeyTrackerBase\\Test_2\\f_Owl_Icon.ico");

            //show system tray icon and assign the icon for it
            systemTrayIcon = new NotifyIcon();
            systemTrayIcon.Icon = applicationIcon;
            systemTrayIcon.Visible = true;

            //creates the menu in the system tray icon
            MenuItem quitMenuItem = new MenuItem("Quit");
            MenuItem programName = new MenuItem("AntiBullying Prototype v0.3");
            MenuItem settingsItem = new MenuItem("Settings");
            ContextMenu systemTrayMenu = new ContextMenu();
            systemTrayMenu.MenuItems.Add(programName);
            systemTrayMenu.MenuItems.Add(settingsItem);
            systemTrayMenu.MenuItems.Add(quitMenuItem);
            systemTrayIcon.ContextMenu = systemTrayMenu;

            //creates events for the specific menu items
            quitMenuItem.Click +=quitMenuItem_Click;
            settingsItem.Click += settingsItem_Click;

            InitializeComponent();
#endregion
            
            #region Keys to watch
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new System.Windows.Forms.KeyEventHandler(gkh_KeyUp);
            //automatically detects keys inputted for use in KeyDown event below
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                gkh.HookedKeys.Add(key);

            #endregion

        }
        //opens settings form on click in menu
        void settingsItem_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.Show();
        }
        //quits the program
        void quitMenuItem_Click(object sender, EventArgs e)
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
            RoutedEventArgs f = new RoutedEventArgs();

            //counts characters
            countChar++;

            #region Spacebar
            if (e.KeyCode == Keys.Space)
                countWord++;
            #endregion

            #region Shift - NOT WORKING (YET)
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
                //shift = true;//turns shift on for next char
            #endregion

            #region Backspace
            if (e.KeyCode == Keys.Back)
            {
                if (sentence != "")
                {
                    sentence = sentence.Remove(sentence.Length - 1);
                    textBox1.Text = sentence;
                    e.Handled = false;
                }
            }
            #endregion

            #region Return
            if (e.KeyCode == Keys.Return)
            {

            }
            #endregion

            

            //MIGHT BE WORTH ADDING A VARYING TIMER TO RECORD TEXT IN THE PAST _ MINUTES
            #region New Key Logger (Main) - User

            if (e.KeyCode != Keys.Back)
            {
                sentence += ((char)e.KeyValue).ToString().ToLower();
                textBox1.Text = sentence;

                #region Word Checker

                    ////hierarchy level of words
                    //switch (level)
                    //{
                    //    case "low":

                    //        break;
                    //    case "medium":

                    //        break;
                    //    case "High":

                    //        break;
                    //    default:

                    //        break;
                    //}

                #endregion

                #region Word Detector

                if (sentence != "\r")
                    {
                        if (sentence != "¾")
                        {
                            if (countWord == 30 || countChar == 180 || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Return)
                            {
                                if (sentence != "")
                                {
                                    using (StreamWriter file = new StreamWriter(@"C:\\Users\\Dave\\Documents\\GitHub\\GroupProjectUpdated\\KeyTrackerBase\\log.txt", true))
                                    {
                                        if (e.KeyCode != Keys.OemPeriod)
                                        {
                                            //writes entries to txt file - logger
                                            file.WriteLine(DateTime.Now.ToString() + ":  " + sentence);
                                            //word detection - NOT WORKING
                                            for (
                                                int i = 0; i < words.Length; i++)
                                            {
                                                if (sentence.Contains(words[i]))
                                                {
                                                    countDetect++;
                                                    badWords += words[i] + " ";

                                                    
                                                }

                                            }
                                            file.WriteLine("Amount of Words Detected: " + countDetect + " - Words: " + badWords);
                                                    //capture screenshot & send email with it and log message
                                                    //http://www.mindfiresolutions.com/How-to-take-screenshot-programmatically-and-mail-it-in-C-647.php
                                                    Bitmap screencapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                                                    Graphics graphics = Graphics.FromImage(screencapture as Image);
                                                    graphics.CopyFromScreen(0, 0, 0, 0, screencapture.Size);
                                                    screencapture.Save(@"C:\\Users\\Dave\\Desktop\\screenshot" + iScreen + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    
                                                    SendEmail("cyberbullyingauthority@gmail.com", "User: " + userName + badWords, "Sentence: " + sentence, @"C:\\Users\\Jon\\Desktop\\screenshot" + iScreen + ".jpeg");
                                                    iScreen++;

                                        }
                                        else
                                        {
                                            //writes entries to txt file, removes period symbol and puts fullstop - logger
                                            file.WriteLine(DateTime.Now.ToString() + ":  " + sentence.Remove(sentence.Length - 1) + ".");
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
                #endregion
            }

            #endregion

            

            

        }


        void gkh_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Put keyup events in here (if necessary)
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void SendEmail(string to, string subject, string body, string path)
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
                message.Attachments.Add(new Attachment(path));

                //------------------------------end----------------------------------------------

                //------------------------------Email client used (gmail)------------------------------

                //crearting client object with gmail smtp details and port
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //login credentials of screenshot sender
                client.Credentials = new System.Net.NetworkCredential("screenshotbully@gmail.com", "cyberbullying");
                //enabling secure connection
                client.EnableSsl = true;
                //sending the email message
                client.Send(message);

                //------------------------------end-----------------------------------------------
                //freeing memory
                message = null;
            }
            catch (Exception Ex)
            {
                //message = null;
                throw Ex;
            }
        }

    }
}
