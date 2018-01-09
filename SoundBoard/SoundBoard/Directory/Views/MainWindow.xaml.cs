﻿using System.Windows;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SoundBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
        
        //Drag and drop file allowed or not based on extension
        private void MySounds_DragEnter(object sender, DragEventArgs e)
        {
            bool dropEnabled = true;
            var allowedExtensions = new[] { ".mp3", ".wav", ".flac", ".aac", ".mp4", ".flv", ".wmv", ".mov", ".avi", ".mpeg4" };

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                dropEnabled = filenames.All(fn => allowedExtensions.Contains(Path.GetExtension(fn)));
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        //Drag and drop file handling action
        public void MySounds_Drop(object sender, DragEventArgs e)
        {
            if(sender is System.Windows.Controls.Button)
            {
                e.Handled = true;
                return;
            }
            string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            
            //Specify which MainWindowViewModel you want to target, there is only one but you still need to specify it
            //the DataContext already is set to the MainWindowViewModel
            ((MainWindowViewModel)this.DataContext).AddAudioFiles(files);
        }

        //Supress Alt functionality to enable keybindings with Alt in it
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                e.Handled = true;
                return;
            }
        }
    }
}

