using System.Collections.ObjectModel;

namespace SoundBoard
{
    public class SoundViewModel : BaseViewModel
    {
        #region Public Properties

        //gets the name of the file, without it's directory
        public string Name { get { return FolderContents.GetFileName(this.AudioLocation); } }
        //gets the name of the file, without directory or extention
        public string NormalizedName { get { return System.IO.Path.GetFileNameWithoutExtension(this.AudioLocation); } }
        //location of the file, full path
        public string AudioLocation { get; set; }
        
        #endregion

        #region Constructor

        public SoundViewModel(string audioLocation)
        {
            this.AudioLocation = audioLocation;
        }

        #endregion
    }
}