using System.Windows;
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

        private void modifier_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Easily add the modifier keys by just pressing them when typing in the textfield
            //If alt is pressed first, the field will lose focus

            //If either Ctrl key is pressed
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                //If Textfield is empty add "Ctrl", else "+Ctrl"
                if (modifierField.Text == "")
                {
                    modifierField.Text = "Ctrl";
                    e.Handled = true;
                }
                //If Textfield already containts Ctrl, return
                if (modifierField.Text.Contains("Ctrl") || modifierField.Text.Contains("+Ctrl"))
                {
                    return;
                }
                modifierField.Text += "+Ctrl";
                e.Handled = true;
            }
            //If either Alt key is pressed
            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                //If Textfield is empty add "Alt", else "+Alt"
                if (modifierField.Text == "" || modifierField.Text == null)
                {
                    modifierField.Text = "Alt";
                    e.Handled = true;
                }
                //If Textfield already contains Alt, return
                if (modifierField.Text.Contains("Alt") || modifierField.Text.Contains("+Alt"))
                {
                    return;
                }
                modifierField.Text += "+Alt";
                e.Handled = true;
            }
            //If either Shift key is pressed
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                //If Textfield is empty add "Shift", else "+Shift"
                if (modifierField.Text == "")
                {
                    modifierField.Text = "Shift";
                    e.Handled = true;
                }
                //If Textfield already contains Shift, return
                if (modifierField.Text.Contains("Shift") || modifierField.Text.Contains("+Shift"))
                {
                    return;
                }
                modifierField.Text += "+Shift";
                e.Handled = true;
            }
        }
    }
}
