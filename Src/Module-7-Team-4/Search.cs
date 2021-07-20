using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Module_7_Team_4
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
           // this.Paint += new PaintEventHandler(set_background);
            PauseButton.Enabled = false;
            StopButton.Enabled = false;

        }

        string selection;
        string selectedItem = "";

        ManualResetEvent pauseEvent = new ManualResetEvent(true);


        class UserState
        {
            public string extension;

            public UserState(string ex)
            {
                extension = ex;
            }
        }

        //you will need System.IO and System.Diagnostics
        //Change the configuration to Debug to see a list of folders that are being read.
        //Change to release to see just the folders that cannot be read.
        //The search will be faster in Release configuration


        delegate void AddFilesDelegate(FileInfo[] Files);

        private void Find(string extension)
        {
            
            foreach (String drive in Directory.GetLogicalDrives())
            {
                Debug.WriteLine(drive);
                foreach (DirectoryInfo child in getDirectories(drive))
                {
                    UserState state = new UserState(extension);
                    this.backgroundWorker.ReportProgress(0, state);
                    extension = state.extension;

                    Debug.WriteLine(child.FullName);
                    FindFiles(child, extension);
                    
                }
            }
        }

        private void FindFiles(DirectoryInfo dir, string extension)
        {
            AddFilesDelegate addFiles = new AddFilesDelegate(AddFiles);
            try
            {
                DirectoryInfo[] children = getDirectories(dir);

                if (this.backgroundWorker.CancellationPending) { return; }
                pauseEvent.WaitOne();

                UserState state = new UserState(extension);
                this.backgroundWorker.ReportProgress(0, state);
                extension = state.extension;

                if (children.Length > 0)
                {
                    foreach (DirectoryInfo child in children)
                    {
                        Debug.WriteLine(child.FullName);
                        FindFiles(child,extension);
                    }
                }
                else
                {
                    //gets .html files when trying to get hml files
                    FileInfo[] Files = dir.GetFiles("*" + extension);
                    if (Files.Length > 0)
                    {
                        this.Invoke(addFiles, new object[] { Files });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private bool AttrOn(FileAttributes attr, FileAttributes field)
        {
            return (attr & field) == field;
        }

        public DirectoryInfo[] getDirectories(DirectoryInfo dir)
        {
            if (AttrOn(dir.Attributes, FileAttributes.Offline))
            {
                Console.Out.WriteLine(dir.Name + " is not mapped ");
                return new DirectoryInfo[] { };
            }
            if (!dir.Exists)
            {
                Console.Out.WriteLine(dir.Name + " does not exist ");
                return new DirectoryInfo[] { };
            }
            try
            {
                return dir.GetDirectories();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                Console.Out.WriteLine(ex.StackTrace);
                return new DirectoryInfo[] { };
            }
        }

        public DirectoryInfo[] getDirectories(String strDrive)
        {
            DirectoryInfo dir = new DirectoryInfo(strDrive);
            return getDirectories(dir);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            PauseButton.Enabled = true;
            StopButton.Enabled = true;

            listBox.Items.Clear();

            selection = (string)comboBox.SelectedItem;

            this.backgroundWorker.RunWorkerAsync((string)selection);
        }


        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (PauseButton.Text == "Pause")
            {
                selection = (string)comboBox.SelectedItem;
                PauseButton.Text = "Continue";
                pauseEvent.Reset();
            }
            else
            {
                PauseButton.Text = "Pause";
                pauseEvent.Set();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {

            PauseButton.Text = "Pause";
            PauseButton.Enabled = false;
            StopButton.Enabled = false;

            pauseEvent.Set();

            if (this.backgroundWorker.IsBusy)
            {
                this.backgroundWorker.CancelAsync();

            }
        }

        private void AddFiles(FileInfo[] Files)
        {
            for (int i = 0; Files.Length > i; i++)
            {
                listBox.Items.Add(Files[i].FullName);
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
            }

            Find((string)e.Argument);      
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedItem = listBox.SelectedItem.ToString();
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PauseButton.Text = "Pause";
            PauseButton.Enabled = false;
            StopButton.Enabled = false;

            MessageBox.Show("Files Found:" + listBox.Items.Count.ToString());

            StartButton.Enabled = true;
        }

        //TODO: OPEN NEW WINDOW AND IMPORT FILE FROM LISTBOX
        //selectedItem is the full path of the file
        private void ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            AddTextDialog dlg = new AddTextDialog();
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(selectedItem);

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    dlg.Add(line);
                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            dlg.Show();
        }

        private void Search_FormClosing(object sender, FormClosingEventArgs e)
        {
            pauseEvent.Set();
            this.backgroundWorker.CancelAsync();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UserState progress = (UserState)e.UserState;
            progress.extension = selection;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selection = (string)comboBox.SelectedItem;
        }

        /*
private void set_background(Object sender, PaintEventArgs e)
{
Graphics graphics = e.Graphics;

//the rectangle, the same size as our Form
Rectangle gradient_rectangle = new Rectangle(0, 0, Width, Height);

//define gradient's properties
Brush b = new LinearGradientBrush(gradient_rectangle, Color.FromArgb(255, 100, 100), Color.FromArgb(200, 128, 100), 65f);

//apply gradient         
graphics.FillRectangle(b, gradient_rectangle);
}*/
    }
}
