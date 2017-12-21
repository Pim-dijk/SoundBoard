using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for ShowKeybindings.xaml
    /// </summary>
    public partial class ShowKeybindingsView : Window
    {
        public ShowKeybindingsView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }
    }
}
