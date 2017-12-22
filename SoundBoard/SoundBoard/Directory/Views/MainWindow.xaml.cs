using System.Windows;
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


        //Drag and drop buttons around, doesn't work in conjunction with the existing item drop
        // will have to merge the 2 together since the parent controll now takes over and that does not allow
        // me to drop a button because it is not a audio/video file.
        //private void ButtonPreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (sender is System.Windows.Controls.Button)
        //        {
        //            System.Windows.Controls.Button draggedItem = sender as System.Windows.Controls.Button;
        //            DragDrop.DoDragDrop(draggedItem, sender, DragDropEffects.Move); // -----fix datacontext----- \\
        //            e.Handled = true;
        //        }
        //    }
        //}

        //private void Button_Drag_Drop(object sender, DragEventArgs e)
        //{
        //    SoundViewModel droppedData = e.Data.GetData(typeof(SoundViewModel)) as SoundViewModel;
        //    SoundViewModel target = ((System.Windows.Controls.Button)(sender)).DataContext as SoundViewModel;

        //    ((MainWindowViewModel)this.DataContext).ArrangeItems(droppedData, target);
        //}
    }
}

