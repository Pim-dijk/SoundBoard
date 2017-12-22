using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for AddKeyBinding.xaml
    /// </summary>
    public partial class AddKeyBindingView : Window
    {
        public AddKeyBindingView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }

        //If tab is pressed in key field, focus to modifier field.
        private void key_KeyPressDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Tab)
            {
                e.Handled = true;
                modifierField.Focus();
            }
            else
            {
                keyField.Text = e.Key.ToString();
                e.Handled = true;
            }
        }

        private void modifier_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Easily add the modifier keys by just pressing them when typing in the textfield
            //If alt is pressed first, the field will lose focus
           
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                if(modifierField.Text == "")
                {
                    modifierField.Text = "Ctrl";
                }
                else if(modifierField.Text.Contains("Ctrl"))
                {
                    return;
                }
                else if (modifierField.Text.Contains("Shift+Alt"))
                {
                    modifierField.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if(modifierField.Text.Contains("Shift"))
                {
                    modifierField.Text = "Ctrl+Shift";
                    return;
                }
                else if(modifierField.Text.Contains("Alt"))
                {
                    modifierField.Text = "Ctrl+Alt";
                    return;
                }
            }

            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (modifierField.Text == "")
                {
                    modifierField.Text = "Shift";
                }
                else if (modifierField.Text.Contains("Shift"))
                {
                    return;
                }
                else if (modifierField.Text.Contains("Ctrl+Alt"))
                {
                    modifierField.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if (modifierField.Text.Contains("Ctrl"))
                {
                    modifierField.Text = "Ctrl+Shift";
                    return;
                }
                else if (modifierField.Text.Contains("Alt"))
                {
                    modifierField.Text = "Shift+Alt";
                    return;
                }
            }

            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                if (modifierField.Text == "")
                {
                    modifierField.Text = "Alt";
                }
                else if (modifierField.Text.Contains("Alt"))
                {
                    return;
                }
                else if (modifierField.Text.Contains("Ctrl+Shift"))
                {
                    modifierField.Text = "Ctrl+Shift+Alt";
                    return;
                }
                else if (modifierField.Text.Contains("Shift"))
                {
                    modifierField.Text = "Shift+Alt";
                    return;
                }
                else if (modifierField.Text.Contains("Ctrl"))
                {
                    modifierField.Text = "Ctrl+Alt";
                    return;
                }
            }
            
        }

        //
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.System && e.OriginalSource is TextBox)
            {
                e.Handled = true;
            }
        }

        //Do some of the known checks for filtering invalid Keygestures
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(modifierField.Text == "Shift")
            {
                MessageBox.Show("Shift is not allowed as only modifier!");
                return;
            }

            if (modifierField.Text.Contains("Shift") && keyField.Text.Contains("NumPad"))
            {
                MessageBox.Show("Shift as a modifier is not allowed with NumPad keys");
                return;
            }
        }
    }
}
