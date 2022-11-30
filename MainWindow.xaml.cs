using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        private string[] paths, files; 
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }

        private void playpause_Click(object sender, RoutedEventArgs e)
        {
            if(isPlaying)
            {
                player.LoadedBehavior = MediaState.Pause;
                isPlaying= false;  
            } else
            {
                player.LoadedBehavior = MediaState.Play;
                isPlaying = true;
            }
        }

        private void open_folder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            fileDialog.Multiselect = true;
            
            if (fileDialog.ShowDialog() == true)
            {
                files = fileDialog.SafeFileNames;
                paths = fileDialog.FileNames;

                for (int i=0 ; i < files.Length; i++)
                {
                    lstfiles.Items.Add(files[i]);
                }
            }
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            player.LoadedBehavior= MediaState.Stop;
        }

        private void lstfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            player.Source = new Uri(paths[lstfiles.SelectedIndex]);
            player.LoadedBehavior = MediaState.Play;
            isPlaying = true;
            player.Volume = 1;
            volume.Value = 1;
        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = (double) volume.Value;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                progress.Maximum = (int)player.NaturalDuration.TimeSpan.TotalSeconds;
                progress.Value = (int)player.Position.TotalSeconds;
                tempo_atual.Content = convertToTime(player.Position.TotalSeconds);
                tempo_total.Content = convertToTime(player.NaturalDuration.TimeSpan.TotalSeconds);
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (lstfiles.SelectedIndex < lstfiles.Items.Count -1)
            {
                lstfiles.SelectedIndex = lstfiles.SelectedIndex + 1;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if(lstfiles.SelectedIndex > 0)
            {
                lstfiles.SelectedIndex = lstfiles.SelectedIndex -1 ;
            }
        }

        public string convertToTime(double tempo)
        {
            try
            {
                double minutes = Math.Floor(tempo)/60; //take integral part
                double seconds = (tempo - minutes); //multiply fractional part with 60
                int M = (int)Math.Floor(minutes);
                int S = (int)Math.Floor(seconds);   //add if you want seconds
                string timeFormat = String.Format("{0:00}:{1:00}", M, S);

                return timeFormat;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
