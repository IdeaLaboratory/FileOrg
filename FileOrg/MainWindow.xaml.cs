using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace FileOrg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer time = null;
        private DispatcherTimer countdownTimer = null;
        private DateTime startTime;
        private double inputTimeInterval;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MoveFiles(string source, string destination, string searchOption = null)
        {
            //get files that are need to move
            IEnumerable<string> sFiles;

            if (searchOption != null)
                sFiles = Directory.GetFiles(source, searchOption);
            else
                sFiles = Directory.GetFiles(source);

            foreach (var item in sFiles)
            {
                string destFileFullPath = System.IO.Path.Combine(destination, System.IO.Path.GetFileName(item));
                try
                {
                    File.Move(item, destFileFullPath);
                }
                catch (Exception FileMoveException)
                {
                    Debug.Assert(false, FileMoveException.Message);
                }
            }
        }

        private void sourceButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sourceTB.Text = dialog.SelectedPath;
            }
        }

        private void DestinationButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DestinationTB.Text = dialog.SelectedPath;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StartButton.Content.ToString().Contains("Stop"))
                {
                    time.Stop();
                    countdownTimer.Stop();
                    CountdownTimeLB.Content = "";
                    StartButton.Content = "Start";
                    return;
                }

                //Is Periodically operation?
                bool? isPeriodic = PeriodicallyRB.IsChecked;
                if (isPeriodic == true)
                {
                    //TODO: Validate input

                    //added a timer that will take the interval time as input from user
                    time = new DispatcherTimer();
                    //default interval as 1 minute
                    inputTimeInterval = 1;
                    double.TryParse(IntervalTB.Text, out inputTimeInterval);
                    time.Interval = TimeSpan.FromMinutes(inputTimeInterval);
                    time.Tick += IntervalOperation;
                    //initiate the clock
                    time.Start();


                    //Countdown
                    startTime = DateTime.Now;
                    countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                    countdownTimer.Tick += CoundDownEvent;

                    //timers started
                    time.Start();
                    countdownTimer.Start();

                    StartButton.Content = "Stop";
                }
                else
                {
                    MoveFiles(sourceTB.Text, DestinationTB.Text, FiletypeTB.Text);
                }
            }
            catch(Exception ex)
            {
                Debug.Assert(false, "Error" + ex.Message);
            }

            //timer.Enabled = true;
        }

        /// <summary>
        /// Operation need to perform in time interval
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void IntervalOperation(object source, EventArgs e)
        {
            MoveFiles(sourceTB.Text, DestinationTB.Text, FiletypeTB.Text);
        }

        private void CoundDownEvent(object source, EventArgs e)
        {
            CountdownTimeLB.Content = (TimeSpan.FromSeconds(inputTimeInterval) - (DateTime.Now - startTime)).ToString("hh\\:mm\\:ss");
        }
    }
}
