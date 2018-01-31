using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        //Keybind Key textfield
        private void SoundKeybindKey_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Delete || e.Key == Key.Back)
            {
                e.Handled = true;
                soundKeybindKey.Text = "";
                return;
            }

            //Supress modifier keys
            if(e.Key == Key.System || e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                e.Handled = true;
                return;
            }

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

        //Keybind Modifier textfield
        private void SoundKeybindModifier_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Easily add the modifier keys by just pressing them when typing in the textfield
            //If alt is pressed first, the field will lose focus

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                return;
            }

            if(e.Key == Key.Delete || e.Key == Key.Back)
            {
                e.Handled = true;
                soundKeybindModifier.Text = "";
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

        //Modifier lost focus check
        private void SoundKeybindModifier_LostFocus(object sender, RoutedEventArgs e)
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

        //Volume slider
        private void SoundVolumeSlider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            soundVolumeSlider.Value = 0;
        }

        //Add image
        private void SoundImageButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "All Graphics Types|*.jpg;*.gif;*.bmp;*.png;|All Files(*.*)|*.*",
                Multiselect = false
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageLocation.Text = ofd.FileName;
            }
        }
        
        //Remove image
        private void SoundImageRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ImageLocation.Text == "")
            {
                return;
            }
            else
            {
                ImageLocation.Text = "";
            }
        }

        //Save settings
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        //Supress the default Alt key behaviour
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.OriginalSource is TextBox)
            {
                if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.System)
                {
                    e.Handled = true;
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
        }

        //Search the existing categories and return the suggestions.
        private void SoundCategoryTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            //Get the data from the MainWindowViewModel as this is the datacontext to this view.
            var data = ((dynamic)this.DataContext).CategoryList;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear   
                resultStack.Children.Clear();
                border.Visibility = Visibility.Collapsed;
            }
            else
            {
                border.Visibility = Visibility.Visible;
            }

            // Clear the list   
            resultStack.Children.Clear();

            // Add the result   
            foreach (var obj in data)
            {
                if (obj.ToLower().StartsWith(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work   
                    AddSuggestion(obj);
                    found = true;
                }
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        //Add the found items to the stackpanel dropdown
        private void AddSuggestion(string text)
        {
            TextBlock block = new TextBlock
            {
                // Add the text   
                Text = text,

                // A little style...   
                Margin = new Thickness(2, 3, 2, 3),
                Cursor = Cursors.Hand
            };

            // Mouse events   
            block.MouseLeftButtonUp += (sender, e) =>
            {
                soundCategoryTextBox.Text = (sender as TextBlock).Text;
                hintBox.Visibility = Visibility.Hidden;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock tb = sender as TextBlock;
                tb.Background = System.Windows.Media.Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = System.Windows.Media.Brushes.Transparent;
            };

            // Add to the panel   
            resultStack.Children.Add(block);
        }

        //When clicking outside the dropdown, hide it
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hintBox.Visibility = Visibility.Hidden;
        }
    }
}
