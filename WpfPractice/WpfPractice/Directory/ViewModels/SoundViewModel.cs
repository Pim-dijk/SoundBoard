using System.Collections.ObjectModel;

namespace WpfPractice
{
    public class SoundViewModel : BaseViewModel
    {
        #region Public Properties

        public string Name { get { return FolderContents.GetFileName(this.AudioLocation); } }

        public string NormalizedName { get { return System.IO.Path.GetFileNameWithoutExtension(this.AudioLocation); } }

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
