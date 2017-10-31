using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoundBoard
{
    class FolderContents
    {
        /// <summary>
        /// Create a list with all the audio files found in the directory location
        /// </summary>
        /// <param name="audioLocation">The directory location specified</param>
        /// <returns></returns>
        public static List<Sound> GetFolderContents(string defaultDirectory)
        {
            var items = new List<Sound>();

            try
            {
                //Choose which file extensions get imported
                var files = Directory.EnumerateFiles(defaultDirectory).Where
                    (file => file.ToLower().EndsWith(".mp3") //audio formats
                    || file.ToLower().EndsWith(".wav")
                    || file.ToLower().EndsWith(".mp4") //start of video formats
                    || file.ToLower().EndsWith(".flv")
                    || file.ToLower().EndsWith(".wmv")
                    || file.ToLower().EndsWith(".mov")
                    || file.ToLower().EndsWith(".avi")
                    || file.ToLower().EndsWith(".mpeg4")
                    || file.ToLower().EndsWith(".mpegps"))
                    .ToList();
                
                if (files.Count > 0)
                {
                    items.AddRange(files.Select(file => new Sound { AudioLocation = file}));
                }
            }
            catch { }

            return items;
        }

        /// <summary>
        /// Get all the images in the folder
        /// </summary>
        /// <param name="defaultDirectory">The directory location specified</param>
        /// <returns></returns>
        public static List<string> GetImages(string defaultDirectory)
        {
            var images = new List<string>();

            try
            {
                //List of all images in the directory
                var files = Directory.EnumerateFiles(defaultDirectory).Where
                    (file => file.ToLower().EndsWith(".jpg")
                    || file.ToLower().EndsWith(".png")
                    || file.ToLower().EndsWith(".gif")
                    || file.ToLower().EndsWith(".bmp"))
                    .ToList();
                if(files.Count > 0)
                {
                    images.AddRange(files);
                }
            }
            catch { }

            return images;
        }

        /// <summary>
        /// Get only the filename with its extention
        /// </summary>
        /// <param name="path">The full path to the file</param>
        /// <returns></returns>
        public static string GetFileName(string fullPath)
        {
            //If we have no path, return empty
            if (string.IsNullOrEmpty(fullPath))
            {
                return string.Empty;
            }

            //normalize the path to work across platforms
            var normalizePath = fullPath.Replace('/', '\\');

            var lastIndex = normalizePath.LastIndexOf('\\');
            
            //If we can't find a backslash, return path itself
            if (lastIndex <= 0)
            {
                return fullPath;
            }

            //return name after last backslash
            return fullPath.Substring(lastIndex + 1);
        }
    }
}
