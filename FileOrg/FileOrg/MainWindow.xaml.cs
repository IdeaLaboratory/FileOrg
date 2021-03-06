﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FileOrg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //code
            //string source = @"F:\S", destination = @"F:\D";
        }

        private void MoveFiles(string source, string destination)
        {
            //get files that are need to move
            IEnumerable<string> sFiles = Directory.GetFiles(source);
            foreach (var item in sFiles)
            {
                string destFileFullPath = System.IO.Path.Combine(destination, System.IO.Path.GetFileName(item));
                File.Move(item, destFileFullPath);
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
            //Is Periodically operation?
            bool? isPeriodic = PeriodicallyRB.IsChecked;
            if (isPeriodic == true)
            {
                //TODO: Validate inpute
                //Timer myTimer = new Timer();
                //myTimer.Elapsed += new ElapsedEventHandler(IntervalOperation);
                DispatcherTimer time = new DispatcherTimer();

                double inputTimeInterval = 1;
                double.TryParse(IntervalTB.Text, out inputTimeInterval);
                time.Interval = TimeSpan.FromMinutes(inputTimeInterval);
                time.Start();
                time.Tick += IntervalOperation;
            }
            else
            {
                MoveFiles(sourceTB.Text, DestinationTB.Text);
            }
        }

        /// <summary>
        /// Operation need to perform in time interval
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void IntervalOperation(object source, EventArgs e)
        {
            MoveFiles(sourceTB.Text, DestinationTB.Text);
        }
    }
}
