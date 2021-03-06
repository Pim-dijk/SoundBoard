﻿namespace SoundBoard
{
    public class Sound
    {
        public string Name { get { return FolderContents.GetFileName(this.AudioLocation); } }

        public string NormalizedName { get { return System.IO.Path.GetFileNameWithoutExtension(this.AudioLocation); } }

        public string AudioLocation { get; set; }

        public string ImageBitMap { get; set; }

        public string ImagePath { get; set; }

        public bool HasImage { get; set; }

        public bool IsPlaying { get; set; }

        public string Keybind { get; set; }

        public string Modifier { get; set; }

        public float Volume { get; set; }

        public string Category { get; set; }
    }
}
