using System.Windows;

namespace SoundBoard.Views
{
    /// <summary>
    /// Interaction logic for AddKeyBinding.xaml
    /// </summary>
    public partial class AdjustVolumeView : Window
    {
        public AdjustVolumeView(object dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
