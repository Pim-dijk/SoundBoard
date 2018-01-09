using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for SoundSettings.xaml
    /// </summary>
    public partial class SoundSettingsView : Window
    {
        public SoundSettingsView(object dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
        }

        private void soundKeybindKey_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                soundKeybindModifier.Focus();
            }
            else
            {
                soundKeybindKey.Text = e.Key.ToString();
                e.Handled = true;
            }
        }

        private void soundKeybindModifier_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Easily add the modifier keys by just pressing them when typing in the textfield
            //If alt is pressed first, the field will lose focus

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                if (soundKeybindModifier.Text == "")
                {
                    soundKeybindModifier.Text = "Ctrl";
                }
                else if (soundKeybindModifier.Text.Contains("Ctrl"))
                {
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Shift+Alt"))
                {
                    soundKeybindModifier.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Shift"))
                {
                    soundKeybindModifier.Text = "Ctrl+Shift";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Alt"))
                {
                    soundKeybindModifier.Text = "Ctrl+Alt";
                    return;
                }
            }

            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (soundKeybindModifier.Text == "")
                {
                    soundKeybindModifier.Text = "Shift";
                }
                else if (soundKeybindModifier.Text.Contains("Shift"))
                {
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Ctrl+Alt"))
                {
                    soundKeybindModifier.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Ctrl"))
                {
                    soundKeybindModifier.Text = "Ctrl+Shift";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Alt"))
                {
                    soundKeybindModifier.Text = "Shift+Alt";
                    return;
                }
            }

            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                if (soundKeybindModifier.Text == "")
                {
                    soundKeybindModifier.Text = "Alt";
                }
                else if (soundKeybindModifier.Text.Contains("Alt"))
                {
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Ctrl+Shift"))
                {
                    soundKeybindModifier.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Shift"))
                {
                    soundKeybindModifier.Text = "Shift+Alt";
                    return;
                }
                else if (soundKeybindModifier.Text.Contains("Ctrl"))
                {
                    soundKeybindModifier.Text = "Ctrl+Alt";
                    return;
                }
            }
        }

        private void soundKeybindModifier_LostFocus(object sender, RoutedEventArgs e)
        {
            if (soundKeybindModifier.Text == "Shift")
            {
                MessageBox.Show("Shift is not allowed as only modifier!");
                return;
            }

            if (soundKeybindModifier.Text.Contains("Shift") && soundKeybindKey.Text.Contains("NumPad"))
            {
                MessageBox.Show("Shift as a modifier is not allowed with NumPad keys");
                return;
            }
        }

        private void soundVolumeSlider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            soundVolumeSlider.Value = 1;
        }

        private void soundImageButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "All Graphics Types|*.jpg;*.gif;*.bmp;*.png;|All Files(*.*)|*.*";
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageLocation.Text = ofd.FileName;
            }
        }
        
        private void soundImageRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ImageLocation.Text == "")
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove the image, it will be deleted from the directory aswell.", "Caustion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ImageLocation.Text = "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
