using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;
using System.Configuration;
using Microsoft.VisualBasic.FileIO;
using SoundBoard.Services;
using System.Threading.Tasks;

namespace SoundBoard
{
    public partial class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        //Create new instance of the MediaPlayer for audio/video playback
        private MediaPlayer mediaPlayer = new MediaPlayer();

        //Set the default folder location, want this to be changeable via application
        private string defaultDirectory;

        private ObservableCollection<string> statusListView;
        #endregion

        #region Properties

        //Create an accessible object to display the time in a label
        public string TimeLabel { get; set; }
        
        //Create a list for all the files in the folder
        public ObservableCollection<SoundViewModel> Sounds { get; set; }
        
        //Default Directory for the application to get the files from
        public string DefaultDirectory
        {
            get
            {
                return this.defaultDirectory;
            }
            set
            {
                if (this.defaultDirectory == value)
                {
                    return;
                }
                else
                {
                    this.defaultDirectory = value;
                }
            }
        }
        
        //Status list view
        public ObservableCollection<string> StatusListView
        {
            get
            {
                return this.statusListView;
            }
            private set
            {
                if(this.statusListView == value)
                {
                    return;
                }
                else
                {
                    this.statusListView = value;
                }
            }                           
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            //Initialize the commands
            InitializeCommands();
            //Initialize collection lists
            InitializeCollections();

            //Sets the directory to the appSettings value
            ReadCustomSettings();
            
            //Gets the files from the directory
            GetFiles();

            StatusListView.Clear();
            WriteStatusEntry("Application loaded");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to get the files from the directory
        /// </summary>
        public void GetFiles()
        {
            this.Sounds = new ObservableCollection<SoundViewModel>
                (FolderContents.GetFolderContents(this.defaultDirectory).Select(content => new SoundViewModel(content.AudioLocation)));
        }

        private void ReadCustomSettings()
        {
            if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultDirectory"]) == false)
            {
                DefaultDirectory = ConfigurationManager.AppSettings["DefaultDirectory"];
            }
        }

        private void WriteStatusEntry(string statusText)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                StatusListView.Insert(0, DateTime.Now + ": " + statusText);
            }));
        }
        #endregion

        #region Initialize Collections
        private void InitializeCollections()
        {
            StatusListView = new ObservableCollection<string>();
        }
        #endregion
        
        #region Command Initialization
        //public CommandBase PlaySound { get; set; }
        public RelayCommand PlaySound { get; set; }
        public RelayCommand StopSound { get; set; }
        public CommandBase ChangeDefaultDirectory { get; set; }
        public RelayCommand AddSound { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand AddFolder { get; set; }
        public RelayCommand RefreshFiles { get; set; }
        
        #region Initialize Commands
        private void InitializeCommands()
        {
            //PlaySound = new CommandBase(PlaySound_Executed);
            PlaySound = new RelayCommand(PlaySound_Executed);
            StopSound = new RelayCommand(StopSound_Executed);
            ChangeDefaultDirectory = new CommandBase(ChangeDefaultDirectory_Executed);
            AddSound = new RelayCommand(AddSound_Executed);
            ExitCommand = new RelayCommand(ExitCommand_Executed);
            AddFolder = new RelayCommand(AddFolder_Executed);
            RefreshFiles = new RelayCommand(RefreshFiles_Executed);
        }
        #endregion

        #endregion

        #region Commands

        #region Play Sound
        /// <summary>
        /// executes the play sound command
        /// </summary>
        /// <param name="param">the tag of the button, which is set with the files name + extention</param>
        private void PlaySound_Executed(object param)
        {
            // Just for testing
            // Outputs which button was pressed, gives param as string
            //System.Diagnostics.Debug.WriteLine($"Clicked: {param as string}");
            
            try
            {
                string tag = param as string;
                string song = defaultDirectory + tag;
                mediaPlayer.Open(new Uri(song));
                mediaPlayer.Play();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();

            }
            catch { }

        }
        #endregion

        #region timer
        /// <summary>
        /// Timer that shows the current and total time of the playing sound
        /// </summary>
        /// <param name="sender">the PlaySound_Executed command</param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source == null)
            {
                TimeLabel = "No file selected...";
            }
            else
            {
                //Check if the file has a timespan before writing it to the label, otherwise throws exception
                if (mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    TimeLabel = String.Format("{0} / {1}", mediaPlayer.Position.ToString(@"mm\:ss"), mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                }
            }
        }
        #endregion

        #region Stop
        /// <summary>
        /// Stop playing the current sound
        /// </summary>
        /// <param name="sender"></param>
        private void StopSound_Executed(object sender)
        {
            mediaPlayer.Stop();
        }
        #endregion

        #region Change Directory
        /// <summary>
        /// Used to change the default directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangeDefaultDirectory_Executed(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog pathSave = new System.Windows.Forms.FolderBrowserDialog();

            //If the config is empty set browser dialog path to...
            if(string.IsNullOrEmpty(DefaultDirectory))
            {
                pathSave.SelectedPath = @"C:\Temp";
            }
            else
            {
                pathSave.SelectedPath = DefaultDirectory;
            }

            //If result is ok...
            if(pathSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                await Task.Factory.StartNew(() =>
                {
                    WriteStatusEntry("Changing directory...");
                    //Set default directory to the newly selected folder
                    DefaultDirectory = pathSave.SelectedPath.ToString() + "\\";

                    //Save the default directory to the config file
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["DefaultDirectory"].Value = DefaultDirectory;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                    WriteStatusEntry("Retreving files from new directory...");
                    GetFiles();

                }).ContinueWith((t2) =>
                {
                    WriteStatusEntry("Directory changed, contents updated.");
                });
            }
        }
        #endregion

        #region Add Sound
        /// <summary>
        /// Open filedialog and add the selected file to the Songs collection
        /// </summary>
        /// <param name="sender"></param>
        private void AddSound_Executed(object sender)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Mp3 files (*.mp3*)|*.mp3";
            ofd.Multiselect = true;

            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //For each selected file...
                foreach (String sound in ofd.FileNames)
                {
                    //set the location of the file that has to be moved and it's destination
                    string fileLocation = sound;
                    string fileDestination = DefaultDirectory + FolderContents.GetFileName(sound);
                    //Try to copy the file the the destination, prompt for override if necessary
                    try
                    {
                        FileSystem.CopyFile(fileLocation, fileDestination, UIOption.AllDialogs, UICancelOption.DoNothing);
                            
                        //If file has been succesfully moved, add it to the list.
                        SoundViewModel s1 = new SoundViewModel(sound);

                        //Check if file already exists in the collection
                        if(!Sounds.Any(x => x.Name == s1.Name ))
                        {
                            Sounds.Add(s1);
                            WriteStatusEntry("File(s) succesfully added.");
                        }
                        else
                        {
                            WriteStatusEntry("File " + s1.Name + " already exists.");
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        DialogService.ShowErrorMessageBox("File could not be found or has it's access restricted.");
                    }
                    catch (IOException)
                    {
                        DialogService.ShowErrorMessageBox("Can not copy file to the same directory!");
                    }
                    catch(UnauthorizedAccessException)
                    {
                        DialogService.ShowErrorMessageBox("You do not have permissions to access the file or folder.");
                    }
                    catch
                    {
                        DialogService.ShowErrorMessageBox("404 error not found.");
                    }
                }
            }
        }
        #endregion

        #region Add Folder
        private void AddFolder_Executed(object sender)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
           
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Create new collection
                ObservableCollection<SoundViewModel> newSounds;
                //Fill collection with the contents of the selected folder
                newSounds = new ObservableCollection<SoundViewModel>
                 (FolderContents.GetFolderContents(fbd.SelectedPath).Select(content => new SoundViewModel(content.AudioLocation)));
                
                //Check if the files already exist in the current collection
                var newSound = newSounds.Where(x => !Sounds.Any(y => x.Name == y.Name));
                if(newSound.Count() > 0)
                {
                    foreach (var sound in newSound)
                    {
                        Sounds.Add(sound);

                        var filePath = sound.AudioLocation;
                        var destPath = DefaultDirectory + sound.Name;
                        FileSystem.CopyFile(filePath, destPath, UIOption.AllDialogs, UICancelOption.DoNothing);
                        WriteStatusEntry(sound.Name + " added");
                        
                    }
                }
                WriteStatusEntry("Import completed.");
            }
        }
        #endregion

        #region Refresh list
        public void RefreshFiles_Executed(object sender)
        {
            GetFiles();
            WriteStatusEntry("List refreshed.");
        }
        #endregion

        #region Exit
        private void ExitCommand_Executed(object sender)
        {
            Application.Current.Shutdown();
        }
        #endregion
        #endregion
    }
}