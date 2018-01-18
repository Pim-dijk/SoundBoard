using System;
using System.Windows.Media;

namespace SoundBoard
{
    [Serializable]
    public class SoundViewModel : BaseViewModel
    {
        #region Public Properties

        //gets the name of the file, without it's directory
        public string Name { get { return FolderContents.GetFileName(this.AudioLocation); } }
        //gets the name of the file, without directory or extention
        public string NormalizedName { get { return System.IO.Path.GetFileNameWithoutExtension(this.AudioLocation); } }
        //location of the file, full path
        public string AudioLocation { get; set; }
        //image file associated with this sound
        public ImageSource ImageBitMap { get; set; }
        //image file path
        public string ImagePath { get; set; }
        //has image
        public bool HasImage { get; set; }
        //Is playing
        public bool IsPlaying { get; set; }
        //Assigned keybinding
        public string Keybind { get; set; }
        //Assiciated Modifier
        public string Modifier { get; set; }
        //Adjusted volume
        public float Volume { get; set; }
        //Category
        public string Category { get; set; }
        #endregion

        #region Constructor

        public SoundViewModel(string audioLocation)
        {
            this.AudioLocation = audioLocation;
            Volume = 1;
            Category = "";
        }

        #endregion
    }
}