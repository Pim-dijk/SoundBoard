using System;

namespace SoundBoard
{
    class FileSizeToStringConverter
    {
        private static readonly string[] Units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public string Convert(long value)
        {
            if (value == 0)
                throw new ArgumentNullException(nameof(value));

            double size = (long)value;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {Units[unit]}";
        }
    }
}
