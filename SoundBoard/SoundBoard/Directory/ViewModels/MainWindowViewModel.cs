﻿using System;
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
using System.Windows.Media.Imaging;
using System.ComponentModel;
using SoundBoard.Views;

namespace SoundBoard
{
    public partial class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        //Create new instance of the MediaPlayer for audio/video playback
        private MediaPlayer mediaPlayer = new MediaPlayer();

        //Create new instance of the MediaElement for audio/video stream playback
        private MediaElement streamPlayer = new MediaElement();

        //Set the default folder location, want this to be changeable via application
        private string defaultDirectory;

        //Create statuslistview collection
        private ObservableCollection<string> statusListView;

        //Timelabel string to display the running time
        private string timeLabel;

        //Volume control
        private double volume;

        //Restore volume
        private double restoreVolume;

        //Muted boolean
        private bool muted;

        //current name used for changing name
        private string currentName { get; set; }

        //Name to display in the namechange dialog
        private string nameToChange { get; set; }

        //Folder watch enabled/disabled
        private bool folderWatch;

        //Array of file extensions (not currently used)
        private String[] extensions = new String[] { ".mp3", ".mp4", ".wav", ".flac", ".aac" };

        private FileSystemWatcher fs;
        
        #endregion

        #region Properties

        //Create an accessible object to display the time in a label
        public string TimeLabel { get; set; }
        
        //Create a list for all the files in the folder
        public ObservableCollection<SoundViewModel> Sounds { get; set; }

        //Omit Imagesource
        public ImageSource ImageSource { get; set; }
        
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

        //Volume
        public double Volume
        {
            get
            {
                if(volume == 0)
                {
                    Muted = true;
                    mediaPlayer.Volume = volume;
                }
                return volume;
            }
            set
            {
                if(this.volume == value)
                {
                    return;
                }

                //Set the volume for the mediaplayer and the slider
                this.volume = value;
                mediaPlayer.Volume = value;
                
                //Dictates the mute button behaviour
                if (volume == 0)
                {
                    Muted = true;
                }
                else
                {
                    Muted = false;
                }
            }
        }

        //Restore Volume
        public double RestoreVolume { get; set; }

        //Muted
        public bool Muted
        {
            get
            {
                return muted;
            }
            set
            {
                if(this.muted == value)
                {
                    return;
                }
                this.muted = value;
            }
        }

        //Current name
        public string CurrentName { get; set; }

        //Current name without extension
        public string NameToChange { get; set; }

        //Folder watch enabled/disabled
        public bool FolderWatch
        {
            get
            {
                return this.folderWatch;
            }
            set
            {
                if(this.folderWatch == value)
                {
                    return;
                }
                this.folderWatch = value;
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
            //Clear the statusListView
            StatusListView.Clear();
            //Write application loaded
            WriteStatusEntry("Application loaded");
            //Set default for timelabel
            TimeLabel = "No file selected...";
            //Initialize folder watcher
            InitializeWatcher();
            //Add eventhandler for when the window closes
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }
        #endregion

        #region Methods

            #region Get files
        /// <summary>
        /// Method to get the files from the directory
        /// </summary>
        public void GetFiles()
        {
            //Get all sounds and create SoundViewModels for each
            this.Sounds = new ObservableCollection<SoundViewModel>
                (FolderContents.GetFolderContents(this.defaultDirectory).Select(content => new SoundViewModel(content.AudioLocation)));

            //Get a list of all the found images
            List<string> images = FolderContents.GetImages(DefaultDirectory);

            //Add the found images to the correct sound
            foreach(var image in images)
            {
                //Get just the name of the image, no path, no extension
                var normImage = Path.GetFileNameWithoutExtension(Path.GetFileName(image));
                //If there is a Sound with the same name add the image to it
                var item = Sounds.FirstOrDefault(i => i.NormalizedName == normImage);
                if (item != null)
                {
                    item.ImagePath = image;
                    item.ImageBitMap = LoadImage(image);
                    item.HasImage = true;
                }
            }
        }
        #endregion

            #region Add Audio files
        public void AddAudioFiles(string[] files)
        {
            string[] sounds = files;

            foreach (String sound in sounds)
            {
                string fileLocation = sound;
                string fileName = FolderContents.GetFileName(sound);
                string fileDestination = DefaultDirectory + fileName;

                //Check if file already exists in the collection...
                if (!Sounds.Any(x => x.Name == fileName))
                {
                    if (fileLocation != fileDestination)
                    {
                        FileSystem.CopyFile(fileLocation, fileDestination, UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    //If file has been succesfully moved, add it to the list.
                    SoundViewModel s1 = new SoundViewModel(sound);
                    Sounds.Add(s1);
                    WriteStatusEntry("File: " + sound + " succesfully added.");

                    //Check if there is an image for this file and add it
                    FindImage(s1.NormalizedName);
                }
            }
        }
        #endregion

            #region FindImage
        /// <summary>
        /// Try to find an image with the same name as the file
        /// </summary>
        /// <param name="justName">The filename for which to search the image for, without extension</param>
        public void FindImage(string justName)
        {
            //The name of the added image
            var soundName = justName;
            //A list of all images in the folder
            var images = FolderContents.GetImages(DefaultDirectory);
            foreach(var image in images)
            {
                //Normalize the name of the image, no path, no extension
                var normImage = Path.GetFileNameWithoutExtension(Path.GetFileName(image));
                if(soundName == normImage)
                {
                    var item = Sounds.FirstOrDefault(i => i.NormalizedName == normImage);
                    item.ImagePath = image;
                    item.ImageBitMap = LoadImage(image);
                    item.HasImage = true;
                    WriteStatusEntry("Image added to the sound file " + justName);
                    return;
                }
            }
        }
        #endregion
        
            #region BitmapConverter
        /// <summary>
        /// Convert a given image to a bitmap image to release it's source for other uses
        /// </summary>
        /// <param name="path">full path to the image</param>
        /// <returns></returns>
        private ImageSource LoadImage(string path)
        {
            var bitmapImage = new BitmapImage();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // optional
            }
            return bitmapImage;
        }
        #endregion
        
        #region Read Application settings
        /// <summary>
        /// Read the stored application settings
        /// </summary>
        private void ReadCustomSettings()
        {
            //If the default directory is not empty or null, read it's value
            if(string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultDirectory"]) == false)
            {
                DefaultDirectory = ConfigurationManager.AppSettings["DefaultDirectory"];
            }

            Volume = SoundBoard.Properties.Settings.Default.Volume;
            FolderWatch = SoundBoard.Properties.Settings.Default.FolderWatcher;
        }
        #endregion

            #region Application closing
        /// <summary>
        /// When the application closes, save the status of the volume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //Store the current value of Volume in the settings
            SoundBoard.Properties.Settings.Default.Volume = Volume;
            //Store the current value of FolderWatcher in the settings
            SoundBoard.Properties.Settings.Default.FolderWatcher = FolderWatch;
            //Save settings
            SoundBoard.Properties.Settings.Default.Save();
        }
        #endregion

            #region Write status entry
        /// <summary>
        /// A listview for status message updates
        /// </summary>
        /// <param name="statusText">The message to display</param>
        private void WriteStatusEntry(string statusText)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
            {
                StatusListView.Insert(0, DateTime.Now + ": " + statusText);
            }));
        }
        #endregion

            #region Initialize watcher
        private void InitializeWatcher()
        {
            //Folder watcher event handler
            fs = new FileSystemWatcher(DefaultDirectory, "*.*");
            fs.EnableRaisingEvents = FolderWatch;
            fs.IncludeSubdirectories = false;
            //This event will check for  new files added to the watching folder
            fs.Created += new FileSystemEventHandler(newfile);
            //this event will check for any deletion of file in the watching folder
            fs.Deleted += new FileSystemEventHandler(fs_Deleted);
        }
        #endregion
        #endregion

        #region Command Initialization

        #region Initialize Collections
        /// <summary>
        /// status view collection
        /// </summary>
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
        public RelayCommand RemoveSound { get; set; }
        public RelayCommand MuteSound { get; set; }
        public RelayCommand DeleteSound { get; set; }
        public RelayCommand AddImage { get; set; }
        public RelayCommand RemoveImage { get; set; }
        public CommandBase OpenAbout { get; set; }
        public RelayCommand ChangeSoundNameSaved { get; set; }
        public RelayCommand OpenChangeName { get; set; }
        public RelayCommand AddStream { get; set; }
        public RelayCommand TestCommand { get; set; }
        public RelayCommand ToggleFolderWatch { get; set; }

        #endregion

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
            RemoveSound = new RelayCommand(RemoveSound_Executed);
            MuteSound = new RelayCommand(MuteSound_Executed);
            DeleteSound = new RelayCommand(DeleteSound_Executed);
            AddImage = new RelayCommand(AddImage_Executed);
            RemoveImage = new RelayCommand(RemoveImage_Executed);
            OpenAbout = new CommandBase(OpenAbout_Executed);
            ChangeSoundNameSaved = new RelayCommand(ChangeSoundNameSaved_Executed);
            OpenChangeName = new RelayCommand(OpenChangeName_Executed);
            AddStream = new RelayCommand(AddStream_Executed);
            TestCommand = new RelayCommand(TestCommand_Executed);
            ToggleFolderWatch = new RelayCommand(ToggleFolderWatch_Executed);
        }
        #endregion
        
        #endregion

        #region Command Execution

            #region Play Sound
        /// <summary>
        /// executes the play sound command
        /// </summary>
        /// <param name="param">the tag of the button, which is set with the files name + extention</param>
        private void PlaySound_Executed(object param)
        {
            //tag is the name of the song.ext
            //Location of the file
            string tag = param as string;
            string song = defaultDirectory + tag;

            //If a sound is already playing stop that one
            var isPlaying = this.Sounds.Where(p => p.IsPlaying == true);
            if(isPlaying.Count() > 0)
            {
                var stopSound = isPlaying.First<SoundViewModel>();
                stopSound.IsPlaying = false;
            }

            //Set the bool to true for the sound that is playing
            var soundToPlay = this.Sounds.Where(s => s.Name == tag);
            if(soundToPlay.Count() > 0)
            {
                var enableSound = soundToPlay.First<SoundViewModel>();
                enableSound.IsPlaying = true;
            }

            //Start playing
            try
            {
                mediaPlayer.Open(new Uri(song));
                mediaPlayer.Play();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            catch(ArgumentNullException anE)
            {
                WriteStatusEntry(anE + "Null exception");
            }
        }
        #endregion

            #region timer
        /// <summary>
        /// Timer that shows the current and total time of the playing sound
        /// </summary>
        /// <param name="sender">the PlaySound_Executed command</param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, EventArgs e)
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
            //If any sound is playing set the bool to false
            var soundToStop = this.Sounds.Where(s => s.IsPlaying == true);
            if(soundToStop.Count() > 0)
            {
                var stopSound = soundToStop.First<SoundViewModel>();
                stopSound.IsPlaying = false;
            }
            //Stop playing
            mediaPlayer.Stop();
        }
        #endregion

            #region Mute
        /// <summary>
        /// Mute or unmute the sound
        /// </summary>
        /// <param name="sender"></param>
        private void MuteSound_Executed(object sender)
        {
            if(Volume != 0)
            {
                RestoreVolume = Volume;
                Volume = 0;
            }
            else
            {
                Volume = RestoreVolume;
            }
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
                String[] files = ofd.FileNames;
                AddAudioFiles(files);
                WriteStatusEntry("Import completed.");
            }
        }
        #endregion

        #region Add stream // needs work
        public void AddStream_Executed(object param)
        {
            Uri uri = new Uri("https://www.youtube.com/v/UmUVXOXiLeM", UriKind.Absolute);
            streamPlayer.Source = uri;
            streamPlayer.Play();
        }
        #endregion

            #region Add Folder
        /// <summary>
        /// Open folderdialog and add all found files to the collection
        /// </summary>
        /// <param name="sender"></param>
        private void AddFolder_Executed(object sender)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Create new collection
                ObservableCollection<SoundViewModel> newSounds = new ObservableCollection<SoundViewModel>();

                //Add all found items with correct extension to collection
                newSounds = new ObservableCollection<SoundViewModel>
                (FolderContents.GetFolderContents(fbd.SelectedPath).Select(content => new SoundViewModel(content.AudioLocation)));

                //Check if the files already exist in the current collection
                var newSound = newSounds.Where(x => !Sounds.Any(y => x.Name == y.Name));
                if (newSound.Count() > 0)
                {
                    foreach (var sound in newSound)
                    {
                        Sounds.Add(sound);

                        var filePath = sound.AudioLocation;
                        var destPath = DefaultDirectory + sound.Name;
                        FileSystem.CopyFile(filePath, destPath, UIOption.AllDialogs, UICancelOption.DoNothing);
                        WriteStatusEntry(sound.Name + " added");
                        //Search for same name image
                        FindImage(sound.NormalizedName);
                    }
                }
                WriteStatusEntry("Import completed.");
            }
        }
        #endregion

            #region Add Image
        /// <summary>
        /// Add image to the sound that requested it
        /// </summary>
        /// <param name="param">Normalized name of the sound that send the request</param>
        private void AddImage_Executed(object param)
        {
            var soundName = param as string;

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "All Graphics Types|*.jpg;*.gif;*.bmp;*.png;|All Files(*.*)|*.*";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //path to the selected file
                FileInfo imageFile = new FileInfo(ofd.FileName);
                //the new imageLocation with original extention but new name
                var newImageLocation = DefaultDirectory + soundName + imageFile.Extension;
                
                //Check if there already is an image with the same name in the folder
                //var location = DefaultDirectory + soundName;
                //List<FileInfo> list = new List<FileInfo>();
                //string[] extensions = { ".png", ".jpg", ".gif", ".bmp" };
                //foreach(string ext in extensions)
                //{
                //    list.AddRange(new DirectoryInfo(DefaultDirectory).GetFiles(soundName + ext).Where(p =>
                //    p.Extension.Equals(ext, StringComparison.CurrentCultureIgnoreCase)).ToArray());
                //}
                //foreach(var image in list)
                //{
                //    File.Delete(image.FullName);
                //    WriteStatusEntry("Existing image removed");
                //}

                //Create bitmapimage from file
                var bitmapImage = LoadImage(ofd.FileName);

                //Check if the image with the same name as the sound exists and add it to the SoundViewModel
                var item = Sounds.FirstOrDefault(i => i.NormalizedName == soundName);
                if (item != null)
                {
                    //Move the file and rename it at the same time
                    imageFile.CopyTo(newImageLocation, true);
                    //Set the imagelocation to the new image
                    item.ImageBitMap = bitmapImage;
                    item.ImagePath = newImageLocation;
                    GetFiles();
                    WriteStatusEntry("New image added");
                }
            }
        }
        #endregion

            #region Open Change Name
        /// <summary>
        /// Opens the change name view
        /// </summary>
        /// <param name="param"></param>
        private void OpenChangeName_Executed(object param)
        {
            //Store the current name
            //Name with extension
            CurrentName = param as string;

            //If the sound is not currently playing...
            if (Sounds.Any(p => p.IsPlaying == false && p.Name == CurrentName))
            {
                //Name without extension
                NameToChange = Path.GetFileNameWithoutExtension(CurrentName);
                WriteStatusEntry(CurrentName);
                //Display the view
                ChangeNameView view = new ChangeNameView(this);
                view.Owner = Application.Current.MainWindow;
                view.ShowDialog();
            }
            else
            {
                WriteStatusEntry("Cannot rename while playing (" + CurrentName + ")");
            }
            
        }
        #endregion

            #region Change file name
        /// <summary>
        /// Save the new name to the file
        /// </summary>
        /// <param name="param"></param>
        private void ChangeSoundNameSaved_Executed(object param)
        {
            //Gets the sound file extension
            var extension = Path.GetExtension(CurrentName);
            var fullCurrentPath = DefaultDirectory + CurrentName;
            
            var newName = NameToChange + extension;
            var sound = Sounds.Where(n => n.Name == CurrentName).First();
            var fullNewPath = DefaultDirectory + newName;

            //If the file has an image attached, rename it aswell
            if(sound.HasImage == true)
            {
                var oldImageName = Path.GetFileName(sound.ImagePath);
                var imageExt = Path.GetExtension(oldImageName);
                var newPath = DefaultDirectory + NameToChange + imageExt;
                var oldPath = sound.ImagePath;
                sound.ImagePath = newPath;
                File.Move(oldPath, newPath);
            }
            sound.AudioLocation = fullNewPath;
            File.Move(fullCurrentPath, fullNewPath);
            
            //Close the window on save
            App.Current.Windows.OfType<ChangeNameView>().First().Close();
        }
        #endregion

        #region Remove Image //needs work
        private void RemoveImage_Executed(object param)
        {
            var soundName = param as string;
            var sound = Sounds.Where(s => s.HasImage == true && s.NormalizedName == soundName);
            if(sound.Count() > 0)
            {
                //unset the image and hasimage properties
                var removeImage = sound.First<SoundViewModel>();
                removeImage.ImageBitMap = null;
                removeImage.HasImage = false;
                WriteStatusEntry("Image removed");

                //As of now the image does not get deleted from the folder
                //so on refresh or reloading the application the image comes back.
            }
            else
            {
                WriteStatusEntry("No image found");
            }
        }
        #endregion 

            #region Refresh list
        /// <summary>
        /// Refresh the list of sounds, full check of the directory
        /// </summary>
        /// <param name="sender"></param>
        public async void RefreshFiles_Executed(object sender)
        {
            await Task.Factory.StartNew(() =>
            {
                GetFiles();
            }
            ).ContinueWith((t2) =>
            {
                WriteStatusEntry("List refreshed.");
            });
        }
        #endregion

            #region Remove Sound
        /// <summary>
        /// Remove the sound as sender from the collection of SoundViewModels
        /// </summary>
        /// <param name="param">Name of the sound that requested it</param>
        private void RemoveSound_Executed(object param)
        {
            var sound = param as string;
            var audioLocation = DefaultDirectory + sound;
            //Check if the file exists in the collection
            if (Sounds.Any(x => x.AudioLocation == audioLocation))
            {
                try
                {
                    Sounds.Remove(Sounds.Where(i => i.AudioLocation == audioLocation).Single());
                    WriteStatusEntry("File '" + sound + "' removed from the list.");
                }
                catch
                {
                    WriteStatusEntry("Unknown exception, please try again.");
                }
            }
        }
        #endregion

        #region Delete Sound // needs work
        /// <summary>
        /// Delete the sound that requested it from the directory aswell as the collection
        /// </summary>
        /// <param name="param">Name of the sound that requested it</param>
        private void DeleteSound_Executed(object param)
        {
            if (DialogService.ShowConfirmationMessagebox("Are you sure you want to delete " + param + " from the list and the directory?") == MessageBoxResult.Yes)
            {
                //Remove the sound from the list
                RemoveSound_Executed(param);

                //Remove the file from the directory
                var item = param as string;
                var file = DefaultDirectory + item;
                if (FileSystem.FileExists(file))
                {
                    FileSystem.DeleteFile(file);
                    WriteStatusEntry("File '" + item + "' deleted from directory");
                }
            }
            else
            {
                WriteStatusEntry("Deletion cancelled");
            }
        }
        #endregion

            #region Folder watch
        private void ToggleFolderWatch_Executed(object param)
        {
            if (FolderWatch == false)
            {
                FolderWatch = true;
                fs.EnableRaisingEvents = true;
                fs.IncludeSubdirectories = false;
                WriteStatusEntry("Watching folder for changes: " + FolderWatch.ToString());
            }
            else
            {
                FolderWatch = false;
                fs.EnableRaisingEvents = false;
                fs.IncludeSubdirectories = false;
                WriteStatusEntry("Watching folder for changes: " + FolderWatch.ToString());
            }
        }
        #endregion

            #region Open About
        /// <summary>
        /// Opens the About view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAbout_Executed(object sender, EventArgs e)
        {
            AboutView view = new AboutView(this);
            view.Owner = Application.Current.MainWindow;
            view.ShowDialog();
        }
        #endregion

            #region Exit
        /// <summary>
        /// Closes the application, saves volume state beforehand
        /// </summary>
        /// <param name="sender"></param>
        private void ExitCommand_Executed(object sender)
        {
            SoundBoard.Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }
        #endregion

        #region Test
        /// <summary>
        /// Test command, do anything here
        /// </summary>
        /// <param name="param"></param>
        private void TestCommand_Executed(object param)
        {

        }
        #endregion

        #endregion

        #region Events
        /// <summary>
        /// When a file gets added to the folder
        /// </summary>
        /// <param name="fscreated"></param>
        /// <param name="Eventocc"></param>
        protected void newfile(object fscreated, FileSystemEventArgs Eventocc)
        {
            //Do logic here when a file gets added to the defaultDirectory
            try
            {
                WriteStatusEntry("Added file: " + Eventocc.Name + " to the folder.");
                WriteStatusEntry("Refresh list under 'Edit'.");
            }
            catch (Exception ex)
            {
                WriteStatusEntry("File could not be added to the collection.");
            }
        }
        
        /// <summary>
        /// When a file gets deleted
        /// </summary>
        /// <param name="fschanged"></param>
        /// <param name="changeEvent"></param>
        protected void fs_Deleted(object fschanged, FileSystemEventArgs changeEvent)
        {
            //Do logic here when a file gets deleted from the defaultDirectory
            try
            {
                WriteStatusEntry("Folder contents have been modified, refresh list.");
            }
            catch (Exception ex)
            {
                WriteStatusEntry("Something strange happened in the sounds folder.");
            }
        }
            #endregion
        }
}