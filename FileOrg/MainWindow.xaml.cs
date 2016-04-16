using System;
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
                //TODO: Validate input

                //added a timer that will take the interval time as input from user
                DispatcherTimer time = new DispatcherTimer();
                //default interval as 1 minute
                double inputTimeInterval = 1;
                double.TryParse(IntervalTB.Text, out inputTimeInterval);
                time.Interval = TimeSpan.FromMinutes(inputTimeInterval);
                time.Tick += IntervalOperation;
                //initiate the clock
                time.Start();
            }
            else
            {
                MoveFiles(sourceTB.Text, DestinationTB.Text, FiletypeTB.Text);
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
