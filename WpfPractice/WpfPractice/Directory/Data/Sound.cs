namespace SoundBoard
{
    public class Sound
    {
        public string Name { get { return FolderContents.GetFileName(this.AudioLocation); } }

        public string NormalizedName { get { return System.IO.Path.GetFileNameWithoutExtension(this.AudioLocation); } }

        public string AudioLocation { get; set; }
    }
}
