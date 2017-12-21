using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace SoundBoard
{
    public class KeybindingsViewModel : BaseViewModel
    {
        public string Keybind { get; set; }

        public string Modifier { get; set; }

        public string SoundName { get; set; }

        public KeybindingsViewModel(string keybind, string modifier, string soundName)
        {
            this.Keybind = keybind;
            this.Modifier = modifier;
            this.SoundName = soundName;
        }
    }
}
