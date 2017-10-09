using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoundBoard
{
    class FolderContents
    {
        /// <summary>
        /// Create a list with all the items found in the directory location
        /// </summary>
        /// <param name="audioLocation">The directory location specified</param>
        /// <returns></returns>
        public static List<Sound> GetFolderContents(string audioLocation)
        {
            var items = new List<Sound>();

            try
            {
                //Choose which fileextentions get imported
                var files = Directory.EnumerateFiles(audioLocation).Where
                    (file => file.ToLower().EndsWith(".mp3") 
                    || file.ToLower().EndsWith(".wav")
                    || file.ToLower().EndsWith(".aac")
                    || file.ToLower().EndsWith(".flac"))
                    .ToList();

                if(files.Count > 0)
                {
                    items.AddRange(files.Select(file => new Sound { AudioLocation = file }));
                }
            }
            catch { }

            return items;
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
